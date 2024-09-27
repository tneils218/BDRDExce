namespace BDRDExce.Models.DTOs
{
    public class RegisterDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public IFormFile File { get; set; }
    }
}