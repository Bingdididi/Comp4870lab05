using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RemoveUserFromRoleModel : PageModel
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager;

    public RemoveUserFromRoleModel(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public string Email { get; set; } // Changed from UserId to Email
    [BindProperty]
    public string RoleName { get; set; }

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
