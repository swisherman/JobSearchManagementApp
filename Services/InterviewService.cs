using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class InterviewService
{
    private readonly IMongoCollection<InterviewRecord> _interviews;

    public InterviewService(IOptions<MongoSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

        _interviews = database.GetCollection<InterviewRecord>("Interviews");
    }

    public async Task<List<InterviewRecord>> GetAllAsync()
    {
        return await _interviews.Find(_ => true).ToListAsync();
    }
}