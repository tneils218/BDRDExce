using BDRDExce.Models;

public class Comment
{
    public long Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public int ExamId { get; set; }
    public int? ParentId { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
}