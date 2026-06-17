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

    [BsonElement("jobTitle")]
    public string JobTitle { get; set; } = "";

    [BsonElement("companyName")]
    public string CompanyName { get; set; } = "";

    [BsonElement("dateApplied")]
    public DateTime DateApplied { get; set; } = DateTime.UtcNow;

    [BsonElement("status")]
    public string Status { get; set; } = "Applied";
    // Applied, Followed Up, Interviewing, Offer, Rejected, Withdrawn

    [BsonElement("resumeVersion")]
    public string ResumeVersion { get; set; } = "";
    [BsonElement("coverLetterVersion")]
    public string CoverLetterVersion { get; set; } = "";

    [BsonElement("followUpDate")]
    public DateTime? FollowUpDate { get; set; }
    [BsonElement("contactName")]
    public string ContactName { get; set; } = "";
    [BsonElement("contactEmail")]
    public string ContactEmail { get; set; } = "";

    [BsonElement("notes")]
    public string Notes { get; set; } = "";

    [BsonElement("interviews")]
    public List<InterviewRecord> Interviews { get; set; } = new();

    [BsonElement("created")]
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [BsonElement("lastModified")] 
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}
