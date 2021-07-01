using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DePool.Data;

namespace DePool.Pages.Pools
{
    public class IndexModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public IndexModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pool> Pool { get;set; }

        public async Task OnGetAsync()
        {
            var userId = User.GetUserId();

            Pool = await _context.Pools
                .Where(x => x.Users.Any(u => u.User.Id == userId)).ToListAsync();
        }
    }
}
