namespace BDRDExce.Models.DTOs
{
    public class CourseDto
    {
        public int Id { get; set;}
        public string Desc { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public FileDto File { get; set; }
        
        public List<ExamDto> Exams { get; set; }
    }
}