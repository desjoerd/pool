using Microsoft.AspNetCore.Identity;
using System;

namespace DePool.Data
{
    public class Forecast
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int GameId { get; set; }

        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual Game Game { get; set; }

        public string GetGoalsString() => $"{HomeGoals} - {AwayGoals}";

        public string GetPublicString(string currentUserId) 
            => UserId == currentUserId || DateTime.Now >= Game.DateTime ? $"{HomeGoals} - {AwayGoals}" : $"X - X";
    }
}
