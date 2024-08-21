namespace BDRDExce.Models;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
  public string FullName  { get; set; }
  public string DOB       { get; set; }
  public string AvatarUrl { get; set; }
}