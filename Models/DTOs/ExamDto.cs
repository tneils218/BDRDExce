namespace BDRDExce.Models.DTOs;

public class ExamDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CourseId { get; set; }
    public bool IsComplete { get; set; }

    public ExamDto(string title, string content, int courseId, bool isComplete)
    {
        Title = title;
        Content = content;
        CourseId = courseId;
        IsComplete = isComplete;
    }
}