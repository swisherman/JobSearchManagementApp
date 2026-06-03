namespace JobSearchApp.Models
{
    public class InterviewRecord
    {
        public DateTime InterviewDate { get; set; }

        public string InterviewType { get; set; } = "";

        public string InterviewerName { get; set; } = "";

        public string Outcome { get; set; } = "";

        public string Notes { get; set; } = "";
    }
}
