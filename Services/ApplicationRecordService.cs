using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class ApplicationRecordService
{
    private readonly IMongoCollection<ApplicationRecord> _applications;

    public ApplicationRecordService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _applications = database.GetCollection<ApplicationRecord>("applications");
    }

    public async Task<List<ApplicationRecord>> GetAsync() =>
        await _applications.Find(_ => true)
            .SortByDescending(a => a.DateApplied)
            .ToListAsync();

    public async Task<ApplicationRecord?> GetAsync(string id) =>
        await _applications.Find(a => a.Id == id).FirstOrDefaultAsync();

    public async Task<List<ApplicationRecord>> GetByCompanyAsync(string companyId) =>
        await _applications.Find(a => a.CompanyId == companyId)
            .SortByDescending(a => a.DateApplied)
            .ToListAsync();

    public async Task<List<ApplicationRecord>> GetByJobPostingAsync(string jobPostingId) =>
        await _applications.Find(a => a.JobPostingId == jobPostingId)
            .SortByDescending(a => a.DateApplied)
            .ToListAsync();

    public async Task CreateAsync(ApplicationRecord application)
    {
        application.Created = DateTime.UtcNow;
        application.LastModified = DateTime.UtcNow;
        await _applications.InsertOneAsync(application);
    }

    public async Task UpdateAsync(string id, ApplicationRecord application)
    {
        application.LastModified = DateTime.UtcNow;
        await _applications.ReplaceOneAsync(a => a.Id == id, application);
    }

    public async Task DeleteAsync(string id) =>
        await _applications.DeleteOneAsync(a => a.Id == id);
}