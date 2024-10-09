namespace BDRDExce.Models.DTOs
{
    public class ChangeCourseDto
    {
        public int Id { get; set;}
        public string Title { get; set; }
        public string Desc { get; set;}
        public string Label  { get; set; }
        public IFormFile Image { get; set; }
    }

}