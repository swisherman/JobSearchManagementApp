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

    public string Status { get; set; } = "Active";
    public string RepoPath { get; set; } = "";
    public string GitHubUrl { get; set; } = "";

    public bool PortfolioReady { get; set; }

    public string ResumeBullet { get; set; } = "";
    public string Notes { get; set; } = "";

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}