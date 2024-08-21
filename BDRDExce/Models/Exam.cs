namespace BDRDExce.Models;

public class Exam
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Media> Medias { get; set; }
}