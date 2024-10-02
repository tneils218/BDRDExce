namespace BDRDExce.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public string ImageUrl { get; set; }
    public Media Media { get; set; }
    public string MediaId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Exam> Exams { get; set; }
    public string Label { get; set; }
}

