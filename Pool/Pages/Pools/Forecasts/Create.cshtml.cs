using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DePool.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DePool.Pages.Pools.Forecasts
{
    public class CreateModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CreateModel(DePool.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [BindProperty(SupportsGet = true)]
        public int PoolId { get; set; }

        public async Task<IActionResult> OnGet(int? gameId)
        {
            var hasAccess = await _context.PoolUsers.AnyAsync(u => u.User.NormalizedUserName == User.Identity.Name);
            if (!hasAccess)
            {
                return NotFound();
            }

            var userId = User.GetUserId();

            var notForecastedGames = await _context.Games.Where(g => g.PoolId == PoolId)
                .Where(g => !g.Forecasts.Any(f => f.UserId == userId))
                .Where(g => DateTime.Now < g.DateTime)
                .OrderBy(g => g.DateTime)
                .ToListAsync();

            var items = notForecastedGames
                .Select(g => new { Id = g.Id, Text = g.ToDisplayString() })
                .ToList();

            ViewData["GameId"] = new SelectList(items, "Id", "Text", gameId);
            return Page();
        }

        [BindProperty]
        public Forecast Forecast { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.GetUserId();
            var hasAccess = await _context.PoolUsers
                .AnyAsync(u => u.UserId == userId);
            if (!hasAccess)
            {
                return NotFound();
            }

            var isForecasted = await _context.Forecasts
                .AnyAsync(f => f.GameId == Forecast.GameId && f.UserId == userId);

            if (isForecasted)
            {
                ModelState.AddModelError("GameId", "Al voorspeld");
            }

            var isInTime = await _context.Games
                .Where(g => g.Id == Forecast.GameId)
                .Where(g => DateTime.Now < g.DateTime)
                .AnyAsync();

            if (!isInTime)
            {
                ModelState.AddModelError("Game", "Te laat");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Forecast.User = user;

            _context.Forecasts.Add(Forecast);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Me", new { PoolId });
        }
    }
}
