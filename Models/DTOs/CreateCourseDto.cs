namespace BDRDExce.Models.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string UserId { get; set; }
        public string Label { get; set; }
        public IFormFile Image { get; set; }
    }
}