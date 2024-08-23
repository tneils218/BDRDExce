namespace BDRDExce.Models.DTOs
{
    public class ChangePasswordDto : BaseLoginDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}