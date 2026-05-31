using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSearchApp.Models;

public class ApplicationRecord
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string JobPostingId { get; set; } = "";
    public string CompanyId { get; set; } = "";

    public string JobTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";

    public DateTime DateApplied { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Applied";
    // Applied, Followed Up, Interviewing, Offer, Rejected, Withdrawn

    public string ResumeVersion { get; set; } = "";
    public string CoverLetterVersion { get; set; } = "";

    public DateTime? FollowUpDate { get; set; }
    public string ContactName { get; set; } = "";
    public string ContactEmail { get; set; } = "";

    public string Notes { get; set; } = "";

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}