using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AddUserToRoleModel : PageModel
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager;

    public List<SelectListItem> Emails { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

    public AddUserToRoleModel(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public string Email { get; set; } // Bind to Email instead of FirstName and LastName

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
        if (string.IsNullOrEmpty(Email))
    {
        ModelState.AddModelError("Email", "Email cannot be null or empty.");
        return Page();
    }
        var user = await _userManager.FindByEmailAsync(Email);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            return RedirectToPage("/Users"); // Redirect back to users list
        }

        var roleExist = await _roleManager.RoleExistsAsync(RoleName);
        if (!roleExist)
        {
            ModelState.AddModelError("", "Role does not exist.");
            return RedirectToPage("/Users"); // Redirect back to users list
        }

        if (await _userManager.IsInRoleAsync(user, RoleName))
        {
            ModelState.AddModelError("", "User already assigned to this role.");
            return RedirectToPage("/Users"); // Redirect back to users list
        }

        var result = await _userManager.AddToRoleAsync(user, RoleName);
        if (result.Succeeded)
        {
            return RedirectToPage("/Users"); // Redirect back to users list on success
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToPage("/Users"); // Redirect back to users list to show errors
        }
    }
}
