using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DePool.Data;

namespace DePool.Pages.Pools.Forecasts
{
    public class MeModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public MeModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int PoolId { get; set; }

        public IList<Forecast> SubmittedForecasts { get;set; }
        public IList<Forecast> DueForecasts { get; set; }
        public IList<Game> MissingForecasts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId();

            var hasAccess = await _context.PoolUsers.AnyAsync(u => u.UserId == userId);
            if(!hasAccess)
            {
                return NotFound();
            }

            var forecasts = await _context.Forecasts
                .Where(f => f.Game.PoolId == PoolId)
                .Where(f => f.UserId == userId)
                .Include(f => f.Game)
                .Include(f => f.User)
                .OrderBy(f => f.Game.DateTime)
                .ToListAsync();

            SubmittedForecasts = forecasts
                .Where(f => DateTime.Now < f.Game.DateTime)
                .ToList();

            DueForecasts = forecasts
                .Where(f => DateTime.Now >= f.Game.DateTime)
                .ToList();

            MissingForecasts = await _context.Games
                .Where(g => g.PoolId == PoolId)
                .Where(g => !g.Forecasts.Any(f => f.User.NormalizedUserName == User.Identity.Name))
                .Where(g => DateTime.Now < g.DateTime)
                .OrderBy(g => g.DateTime)
                .ToListAsync();

            return Page();
        }
    }
}
