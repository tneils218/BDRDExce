namespace BDRDExce.Models.DTOs;

public class ExamDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CourseId { get; set; }
    public bool IsComplete { get; set; }
    public List<FileDto> Files { get; set; }
    

    public ExamDto(int id, string title, string content, int courseId, bool isComplete, List<FileDto> files)
    {
        Id = id;
        Title = title;
        Content = content;
        CourseId = courseId;
        IsComplete = isComplete;
        Files = files;
    }

    public ExamDto()
    {
    }
}