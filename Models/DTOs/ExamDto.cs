namespace BDRDExce.Models.DTOs
{
    public class ExamDto
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public List<Media> Medias { get; set; }
    }
}