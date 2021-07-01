using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(DePool.Areas.Identity.IdentityHostingStartup))]
namespace DePool.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}