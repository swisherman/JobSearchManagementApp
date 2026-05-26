using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;



namespace JobSearchApp.Models
{
    public class JobSearchFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FileName { get; set; } = "";
        public string FullPath { get; set; } = "";
        public string RelativePath { get; set; } = "";
        public string Extension { get; set; } = "";
        public string Category { get; set; } = "Uncategorized";

        public long SizeBytes { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastScanned { get; set; } = DateTime.UtcNow;

        public List<string> Tags { get; set; } = new();
    }
}
