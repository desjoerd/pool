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
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty]
        public UserModel UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            UserModel = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                UserModel = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                return Page();
            }
            
            return RedirectToPage("./Index");
        }
    }
}
