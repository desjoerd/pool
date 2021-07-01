using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DePool.Data;

namespace DePool.Pages.Admin.Pools
{
    public class DeleteModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public DeleteModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pool Pool { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pool = await _context.Pools.FirstOrDefaultAsync(m => m.Id == id);

            if (Pool == null)
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

            Pool = await _context.Pools.FindAsync(id);

            if (Pool != null)
            {
                _context.Pools.Remove(Pool);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
