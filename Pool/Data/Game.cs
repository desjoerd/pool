using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DePool.Data
{
    public class Game
    {
        public int Id { get; set; }

        public int PoolId { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public DateTime DateTime { get; set; }

        public virtual Pool Pool { get; set; }

        public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();

        public string ToDisplayString() => $"{Home} - {Away} {DateTime:(d-M) HHu}";
    }
}
