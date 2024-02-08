using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
    public class RolesModel : PageModel
    {
        private readonly RoleManager<CustomRole> _roleManager; // Use CustomRole if you have additional properties to display

        public RolesModel(RoleManager<CustomRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<CustomRole> RolesList { get; set; } // Change IdentityRole to CustomRole if needed

        public async Task OnGetAsync()
        {
            RolesList = await _roleManager.Roles.ToListAsync() ;
        }
    }

