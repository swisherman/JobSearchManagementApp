using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class JobPostingService
{
    private readonly IMongoCollection<JobPosting> _jobPostings;

    public JobPostingService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _jobPostings = database.GetCollection<JobPosting>("job_postings");
    }

    public async Task<List<JobPosting>> GetAsync() =>
        await _jobPostings.Find(_ => true)
            .SortByDescending(j => j.Created)
            .ToListAsync();

    public async Task<JobPosting?> GetAsync(string id) =>
        await _jobPostings.Find(j => j.Id == id).FirstOrDefaultAsync();

    public async Task<List<JobPosting>> GetByCompanyAsync(string companyId) =>
        await _jobPostings.Find(j => j.CompanyId == companyId)
            .SortByDescending(j => j.Created)
            .ToListAsync();

    public async Task CreateAsync(JobPosting jobPosting)
    {
        jobPosting.Created = DateTime.UtcNow;
        jobPosting.LastModified = DateTime.UtcNow;
        await _jobPostings.InsertOneAsync(jobPosting);
    }

    public async Task UpdateAsync(string id, JobPosting jobPosting)
    {
        jobPosting.LastModified = DateTime.UtcNow;
        await _jobPostings.ReplaceOneAsync(j => j.Id == id, jobPosting);
    }
    
    public async Task DeleteAsync(string id) =>
        await _jobPostings.DeleteOneAsync(j => j.Id == id);
}