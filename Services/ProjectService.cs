using JobSearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobSearchApp.Services;

public class ProjectService
{
    private readonly IMongoCollection<Project> _projects;

    public ProjectService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);

        var database = client.GetDatabase(settings.Value.DatabaseName);

        _projects = database.GetCollection<Project>("projects");
    }

    public async Task<List<Project>> GetAsync()
    {
        return await _projects.Find(_ => true).ToListAsync();
    }

    public async Task CreateAsync(Project project)
    {
        await _projects.InsertOneAsync(project);
    }
}