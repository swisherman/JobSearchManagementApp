using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSearchApp.Models;

public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = "";
    public string WebsiteUrl { get; set; } = "";
    public string CareersUrl { get; set; } = "";
    public string LinkedInUrl { get; set; } = "";

    public string Industry { get; set; } = "";
    public string Location { get; set; } = "";
    public string Notes { get; set; } = "";

    public bool TargetCompany { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}