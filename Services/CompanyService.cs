using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class CompanyService
{
    private readonly IMongoCollection<Company> _companies;

    public CompanyService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _companies = database.GetCollection<Company>("companies");
    }

    public async Task<List<Company>> GetAsync() =>
        await _companies.Find(_ => true).SortBy(c => c.Name).ToListAsync();

    public async Task<Company?> GetAsync(string id) =>
        await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Company company)
    {
        company.Created = DateTime.UtcNow;
        company.LastModified = DateTime.UtcNow;
        await _companies.InsertOneAsync(company);
    }

    public async Task UpdateAsync(string id, Company company)
    {
        company.LastModified = DateTime.UtcNow;
        await _companies.ReplaceOneAsync(c => c.Id == id, company);
    }

    public async Task DeleteAsync(string id) =>
        await _companies.DeleteOneAsync(c => c.Id == id);
}