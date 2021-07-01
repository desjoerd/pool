using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DePool.Data;
using Microsoft.AspNetCore.Identity;

namespace DePool.Pages.Admin.PoolUsers
{
    public class CreateModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public CreateModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["PoolId"] = new SelectList(_context.Pools, nameof(Pool.Id), nameof(Pool.Name));
            ViewData["UserId"] = new SelectList(_context.Users, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
            return Page();
        }

        [BindProperty]
        public PoolUser PoolUser { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PoolUsers.Add(PoolUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
