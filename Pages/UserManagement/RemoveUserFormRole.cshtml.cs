using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class RemoveUserFromRoleModel : PageModel
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager;

    public List<SelectListItem> Emails { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

    public RemoveUserFromRoleModel(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public string Email { get; set; } // Changed from UserId to Email
    [BindProperty]
    public string RoleName { get; set; }

public async Task OnGetAsync()
{
    Emails = await GetEmailsAsync();
    Roles = await GetRolesAsync();
}

private async Task<List<SelectListItem>> GetEmailsAsync()
{
    var users = await _userManager.Users.ToListAsync();
    var emailList = users.Select(u => new SelectListItem
    {
        Value = u.Email,
        Text = u.Email
    }).ToList();

    return emailList;
}

private async Task<List<SelectListItem>> GetRolesAsync()
{
    var roles = await _roleManager.Roles.ToListAsync();
    var roleList = roles.Select(r => new SelectListItem
    {
        Value = r.Name,
        Text = r.Name
    }).ToList();

    return roleList;
}

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByEmailAsync(Email); // Use FindByEmailAsync instead of FindByIdAsync
        var role = await _roleManager.FindByNameAsync(RoleName);

        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            return Page(); // Stay on the page and show an error message
        }
        if (role == null)
        {
            ModelState.AddModelError("", "Role not found.");
            return Page(); // Stay on the page and show an error message
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (result.Succeeded)
        {
            // Optionally redirect to a confirmation page or display a success message
            return RedirectToPage("/Users"); // Redirect back to the user list or a success page
        }
        else
        {
            // Handle failure: log the error, display a message, etc.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToPage("/Users");
        }
    }
}
