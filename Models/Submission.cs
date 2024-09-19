namespace BDRDExce.Models;
public class Submission
{
    public int         Id      { get; set; }
    public string      Content { get; set; }
    public Course      Course    { get; set; }
    public int         CourseId  { get; set; }
    public AppUser     User    { get; set; }
    public string      UserId  { get; set; }
    public List<Media> Medias  { get; set; }
    public string Label { get; set; }
}