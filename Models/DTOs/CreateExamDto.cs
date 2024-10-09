namespace BDRDExce.Models.DTOs;

public class CreateExamDto
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Files { get; set; }

}