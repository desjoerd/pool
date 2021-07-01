using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DePool.Data;
using Microsoft.AspNetCore.Identity;

namespace DePool.Pages.Admin.Users
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public DetailsModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IdentityUser IdentityUser { get; set; }
        public UserModel UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser = await userManager.FindByIdAsync(id);
            if (IdentityUser == null)
            {
                return NotFound();
            }
            UserModel = new UserModel
            {
                Id = IdentityUser.Id,
                UserName = IdentityUser.UserName
            };
            return Page();
        }
    }
}
