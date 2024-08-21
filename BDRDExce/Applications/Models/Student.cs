namespace BDRDExce.Applications.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Points { get; set; }
    public int TeacherId { get; set; }
    public Teacher                 Teacher     { get; set; }
    public ICollection<Submission> Submissions { get; set; }
}
