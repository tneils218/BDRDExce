namespace BDRDExce.Models.DTOs
{
    public class ChangeExamDto
    {
        public int id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool IsComplete { get; set; }
    }
}