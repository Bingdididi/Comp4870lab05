using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AddUserToRoleModel : PageModel
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager;

    public AddUserToRoleModel(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public string Email { get; set; } // Bind to Email instead of FirstName and LastName

    [BindProperty]
    public string RoleName { get; set; }

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
