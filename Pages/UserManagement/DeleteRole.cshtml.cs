using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DeleteRoleModel : PageModel
{
    private readonly RoleManager<CustomRole> _roleManager;

    public DeleteRoleModel(RoleManager<CustomRole> roleManager)
    {
        _roleManager = roleManager;
    }

   public async Task<IActionResult> OnPostAsync(string roleName)
{
    var role = await _roleManager.FindByNameAsync(roleName);
    if (role != null)
    {
        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            // Handle success, perhaps redirect to the roles list with a success message
            TempData["SuccessMessage"] = "Role deleted successfully.";
            return RedirectToPage("/Roles");
        }
        else
        {
            // Handle failure, show error messages
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
    else
    {
        ModelState.AddModelError(string.Empty, "Role not found.");
    }

        return RedirectToPage("/Roles");
    }
}
