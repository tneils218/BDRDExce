namespace BDRDExce.Models.DTOs
{
    public class ExamDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public byte[] ContentFile { get; set; }
        public string ContentComment { get; set; }
        public string ImageUrl { get; set; }
        public int? ParentId { get; set; }
    }
}