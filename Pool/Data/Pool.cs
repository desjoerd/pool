using System.Collections.Generic;

namespace DePool.Data
{
    public class Pool
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PoolUser> Users { get; set; } = new List<PoolUser>();

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
