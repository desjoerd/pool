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
    public class IndexModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<UserModel> UserModel { get;set; }

        public async Task OnGetAsync()
        {
            UserModel = await _context.Users.Select(x => new UserModel { Id = x.Id, UserName = x.UserName }).ToListAsync();
        }
    }
}
