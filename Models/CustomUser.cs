using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Code1stUsersRoles.Models
{
   public class CustomUser : IdentityUser {
  public CustomUser() : base() { }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }


   public string? RoleName { get; set; }
}
}