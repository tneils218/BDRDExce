namespace BDRDExce.Models.DTOs
{
    public class SubmissionDto
    {
        public string Content { get; set; }
        public int ExamId { get; set; }
        public string UserId { get; set; }
        public List<FileDto> Files { get; set; }

        public SubmissionDto()
        {
            
        }

        public SubmissionDto(string content, int examId, string userId, List<FileDto> files)
        {
            Content = content;
            ExamId = examId;
            UserId = userId;
            Files = files;
        }
    }
}