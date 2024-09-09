using System.Security.Cryptography.X509Certificates;

namespace BDRDExce.Models.DTOs
{
    public class ChangeExamDto
    {
        public int Id { get; set;}
        public string Title { get; set; }
        public string Content { get; set;}
        public string UserId { get; set;}
    }

}