using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DePool.Data;

namespace DePool.Pages.Admin.PoolUsers
{
    public class DeleteModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public DeleteModel(DePool.Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PoolUser = await _context.PoolUsers.FindAsync(id);

            if (PoolUser != null)
            {
                _context.PoolUsers.Remove(PoolUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
