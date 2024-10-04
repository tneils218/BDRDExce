namespace BDRDExce.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Media> Medias { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Submission> Submissions { get; set; }
        public bool IsComplete { get; set; }
    }
}