using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AddRoleModel : PageModel
{
    private readonly RoleManager<CustomRole> _roleManager;

    public AddRoleModel(RoleManager<CustomRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [BindProperty]
    public string RoleName { get; set; }
    [BindProperty]
    public string Description { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        CustomRole newRole = new CustomRole(RoleName, Description, DateTime.UtcNow);
        var result = await _roleManager.CreateAsync(newRole);
        if (result.Succeeded)
        {
            // Handle success
            return RedirectToPage("/Roles"); // Redirect to the roles listing page
        }
        else
        {
            // Handle failure
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return Page();
    }
}
