using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class FileScannerService
{
    private readonly IMongoCollection<JobSearchFile> _files;
    private readonly JobSearchSettings _jobSearchSettings;

    public FileScannerService(
        IOptions<MongoSettings> mongoSettings,
        IOptions<JobSearchSettings> jobSearchSettings)
    {
        _jobSearchSettings = jobSearchSettings.Value;

        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

        _files = database.GetCollection<JobSearchFile>("jobSearchFiles");
        var indexKeys = Builders<JobSearchFile>
    .IndexKeys
    .Ascending(f => f.FullPath);

        _files.Indexes.CreateOne(
            new CreateIndexModel<JobSearchFile>(indexKeys));
    }

    public async Task<List<JobSearchFile>> GetAllAsync()
    {
        return await _files
            .Find(_ => true)
            .SortBy(f => f.RelativePath)
            .ToListAsync();
    }
    private static IEnumerable<string> SafeEnumerateFiles(string root)
    {
        var pending = new Queue<string>();
        pending.Enqueue(root);

        while (pending.Count > 0)
        {
            var path = pending.Dequeue();

            string[] files = Array.Empty<string>();
            string[] directories = Array.Empty<string>();

            try
            {
                files = Directory.GetFiles(path);
            }
            catch
            {
            }

            try
            {
                directories = Directory.GetDirectories(path);
            }
            catch
            {
            }

            foreach (var file in files)
                yield return file;

            foreach (var directory in directories)
                pending.Enqueue(directory);
        }
    }
    public async Task<int> ScanAsync()
    {
        var rootFolder = _jobSearchSettings.RootFolder;

        if (string.IsNullOrWhiteSpace(rootFolder))
            throw new InvalidOperationException("JobSearchSettings:RootFolder is not configured.");

        if (!Directory.Exists(rootFolder))
            throw new DirectoryNotFoundException($"Folder not found: {rootFolder}");

        var scannedCount = 0;
        foreach (var filePath in SafeEnumerateFiles(rootFolder))
        //foreach (var filePath in Directory.EnumerateFiles(rootFolder, "*.*", SearchOption.AllDirectories))
        {
            var fileInfo = new FileInfo(filePath);

            var relativePath = Path.GetRelativePath(rootFolder, filePath);

            var fileRecord = new JobSearchFile
            {
                FileName = fileInfo.Name,
                FullPath = fileInfo.FullName,
                RelativePath = relativePath,
                Extension = fileInfo.Extension.ToLowerInvariant(),
                SizeBytes = fileInfo.Length,
                LastModified = fileInfo.LastWriteTimeUtc,
                LastScanned = DateTime.UtcNow,
                Category = GuessCategory(fileInfo),
                Tags = GuessTags(fileInfo)
            };

            var filter = Builders<JobSearchFile>.Filter.Eq(f => f.FullPath, fileRecord.FullPath);

            var existing = await _files
        .Find(filter)
        .FirstOrDefaultAsync();


            if (existing != null &&
        existing.LastModified == fileInfo.LastWriteTimeUtc)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(fileRecord.Id))
            {
                if (existing != null && !string.IsNullOrWhiteSpace(existing.Id))
                {
                    fileRecord.Id = existing.Id;
                }
                else
                {

                    fileRecord.Id = ObjectId.GenerateNewId().ToString();
                }
            }
            await _files.ReplaceOneAsync(
                filter,
                fileRecord,
                new ReplaceOptions { IsUpsert = true });

            scannedCount++;
        }

        return scannedCount;
    }

    private static string GuessCategory(FileInfo fileInfo)
    {
        var name = fileInfo.Name.ToLowerInvariant();
        var extension = fileInfo.Extension.ToLowerInvariant();

        if (name.Contains("resume"))
            return "Resume";

        if (name.Contains("cover"))
            return "Cover Letter";

        if (name.Contains("portfolio"))
            return "Portfolio";

        if (name.Contains("screenshot") || extension is ".png" or ".jpg" or ".jpeg" or ".gif")
            return "Image";

        if (extension is ".pdf")
            return "PDF";

        if (extension is ".docx" or ".doc")
            return "Document";

        if (extension is ".txt" or ".md")
            return "Notes";

        return "Uncategorized";
    }

    private static List<string> GuessTags(FileInfo fileInfo)
    {
        var tags = new List<string>();

        var name = fileInfo.Name.ToLowerInvariant();
        var extension = fileInfo.Extension.ToLowerInvariant();

        if (name.Contains("resume"))
            tags.Add("resume");

        if (name.Contains("cover"))
            tags.Add("cover-letter");

        if (name.Contains("portfolio"))
            tags.Add("portfolio");

        if (extension is ".pdf")
            tags.Add("pdf");

        if (extension is ".docx" or ".doc")
            tags.Add("word");

        if (extension is ".png" or ".jpg" or ".jpeg" or ".gif")
            tags.Add("image");

        return tags.Distinct().ToList();
    }
}