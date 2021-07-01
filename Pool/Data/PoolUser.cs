using Microsoft.AspNetCore.Identity;

namespace DePool.Data
{
    public class PoolUser
    {
        public int Id { get; set; }
        public int PoolId { get; set; }
        public string UserId { get; set; }

        public virtual Pool Pool { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
