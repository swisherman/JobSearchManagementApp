using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class ProjectService
{
    private readonly IMongoCollection<Project> _projects;

    public ProjectService(IOptions<MongoSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

        _projects = database.GetCollection<Project>("projects");
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _projects
            .Find(_ => true)
            .SortBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        return await _projects
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Project project)
    {
        project.Id = ObjectId.GenerateNewId().ToString();
        project.Created = DateTime.UtcNow;
        project.LastModified = DateTime.UtcNow;

        await _projects.InsertOneAsync(project);
    }

    public async Task UpdateAsync(Project project)
    {
        project.LastModified = DateTime.UtcNow;

        await _projects.ReplaceOneAsync(
            p => p.Id == project.Id,
            project);
    }

    public async Task DeleteAsync(string id)
    {
        await _projects.DeleteOneAsync(p => p.Id == id);
    }
}