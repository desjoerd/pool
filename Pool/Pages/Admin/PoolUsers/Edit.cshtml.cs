using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DePool.Data;
using Microsoft.AspNetCore.Identity;

namespace DePool.Pages.Admin.PoolUsers
{
    public class EditModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public EditModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PoolUser PoolUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PoolUser = await _context.PoolUsers
                .Include(p => p.Pool)
                .Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);

            if (PoolUser == null)
            {
                return NotFound();
            }
            ViewData["PoolId"] = new SelectList(_context.Pools, nameof(Pool.Id), nameof(Pool.Name));
            ViewData["UserId"] = new SelectList(_context.Users, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PoolUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoolUserExists(PoolUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PoolUserExists(int id)
        {
            return _context.PoolUsers.Any(e => e.Id == id);
        }
    }
}
