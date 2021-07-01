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
    public class DetailsModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public DetailsModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
