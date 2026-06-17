using MongoDB.Bson.Serialization.Attributes;

namespace JobSearchApp.Models
{
    public class InterviewRecord
    {
        [BsonElement("interviewDate")]
        public DateTime InterviewDate { get; set; }

        [BsonElement("interviewType")]
        public string InterviewType { get; set; } = "";

        [BsonElement("interviewerName")]
        public string InterviewerName { get; set; } = "";

        [BsonElement("outcome")]
        public string Outcome { get; set; } = "";

        [BsonElement("notes")]
        public string Notes { get; set; } = "";
    }
}
