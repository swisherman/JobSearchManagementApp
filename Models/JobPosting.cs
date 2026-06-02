using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSearchApp.Models;

public class JobPosting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Title { get; set; } = "";
    public string CompanyId { get; set; } = "";
    public string CompanyName { get; set; } = "";

    public string PostingUrl { get; set; } = "";
    public string Location { get; set; } = "";
    public string WorkMode { get; set; } = ""; // Remote, Hybrid, On-site

    public string SalaryRange { get; set; } = "";
    public List<string> Technologies { get; set; } = new();

    public string Description { get; set; } = "";
    public string Notes { get; set; } = "";

    public string Status { get; set; } = "Interested";
    // Interested, Applied, Rejected, Interviewing, Closed

    public DateTime? DatePosted { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}