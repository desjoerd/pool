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
    public class IndexModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public IndexModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PoolUser> PoolUser { get;set; }

        public async Task OnGetAsync()
        {
            PoolUser = await _context.PoolUsers
                .Include(p => p.Pool)
                .Include(p => p.User).ToListAsync();
        }
    }
}
