using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSearchApp.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Technologies { get; set; } = new();

    public string RepoPath { get; set; } = "";
    public bool PortfolioReady { get; set; }
}