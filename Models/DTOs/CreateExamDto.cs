namespace BDRDExce.Models.DTOs
{
    public class CreateExamDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string Label { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}