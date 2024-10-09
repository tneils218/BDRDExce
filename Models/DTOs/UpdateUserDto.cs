namespace BDRDExce.Models.DTOs
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string Password { get; set; }
        public IFormFile File { get; set; }
    }
}