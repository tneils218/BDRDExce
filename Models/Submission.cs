namespace BDRDExce.Models;
public class Submission
{
    public int         Id      { get; set; }
    public string      Content { get; set; }
    public Exam        Exam    { get; set; }
    public int         ExamId  { get; set; }
    public AppUser     User    { get; set; }
    public string      UserId  { get; set; }
    public List<Media> Medias  { get; set; }

    public Submission()
    {
        
    }
}