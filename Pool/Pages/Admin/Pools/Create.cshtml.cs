using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DePool.Data;

namespace DePool.Pages.Admin.Pools
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
            return Page();
        }

        [BindProperty]
        public Pool Pool { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Pools.Add(Pool);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
