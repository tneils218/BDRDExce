namespace BDRDExce.Applications.Models;

public class Submission
{
    public int Id { get; set; }
    public string SubmissionContent { get; set; }
    public DateTime SubmittedAt { get; set; }
    public int Points { get; set; }
    public int PostId { get; set; }
    public int StudentId { get; set; }
    public Post Post { get; set; }
    public Student Student { get; set; }
}
