using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


public class UsersModel : PageModel
{
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager; // Inject RoleManager to access role information

    public UsersModel(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IList<UserRoleViewModel> UsersList { get; set; }

    public async Task OnGetAsync()
    {
        var users = _userManager.Users;
        var userRoleViewModelList = new List<UserRoleViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); // Gets the list of roles for the user
            var roleName = roles.FirstOrDefault(); // This assumes the user has only one role. Adjust if that's not the case.
            
            userRoleViewModelList.Add(new UserRoleViewModel
            {
                User = user,
                RoleName = roleName
            });
        }

        UsersList = userRoleViewModelList;
    }
}