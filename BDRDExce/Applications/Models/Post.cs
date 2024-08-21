namespace BDRDExce.Applications.Models;

public class Post
{
    public int      Id        { get; set; }
    public string   Title     { get; set; }
    public string   Content   { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool     IsZip     { get; set; }
    public int TeacherId { get; set; }
    public Teacher                 Teacher     { get; set; }
    public ICollection<Submission> Submissions { get; set; }
}

