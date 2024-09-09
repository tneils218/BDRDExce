namespace BDRDExce.Models.DTOs
{
    public class CreateSubmissionDto
    {
        public string Content { get; set; }
        public int ExamId { get; set; }
        public string UserId { get; set; }
    }
}