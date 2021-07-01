using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DePool.Data;
using DePool.Pages.Admin.Users;

namespace DePool.Pages.Pools.Forecasts
{
    public class EditModel : PageModel
    {
        private readonly DePool.Data.ApplicationDbContext _context;

        public EditModel(DePool.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Forecast Forecast { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.GetUserId();

            Forecast = await _context.Forecasts
                .Include(f => f.Game)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Forecast == null)
            {
                return NotFound();
            }
            
            if (Forecast.UserId != userId)
            {
                return NotFound();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var toUpdate = await _context.Forecasts
                .Include(f => f.Game)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == Forecast.Id);

            if (toUpdate == null)
            {
                return NotFound();
            }
            toUpdate.HomeGoals = Forecast.HomeGoals;
            toUpdate.AwayGoals = Forecast.AwayGoals;

            var userId = User.GetUserId();
            if (toUpdate.UserId != userId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Forecast = toUpdate;
                return Page();
            }

            if (DateTime.Now >= toUpdate.Game.DateTime)
            {
                ModelState.AddModelError("Game", "Te laat");
                Forecast = toUpdate;
                return Page();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForecastExists(Forecast.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Me", new { PoolId = toUpdate.Game.PoolId });
        }

        private bool ForecastExists(int id)
        {
            return _context.Forecasts.Any(e => e.Id == id);
        }
    }
}
