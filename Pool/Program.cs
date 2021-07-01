using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DePool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<SetupRoles>();
                    services.AddHostedService<SetupAdmin>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public class SetupRoles : IHostedService
        {
            private readonly IServiceProvider serviceProvider;

            public SetupRoles(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    if (!await roleManager.RoleExistsAsync("admin"))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole("admin"));
                        if(!result.Succeeded)
                        {
                            throw new DePoolException($"Error Creating admin role {result}");
                        }
                    }
                }
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public class SetupAdmin : IHostedService
        {
            private readonly IServiceProvider serviceProvider;

            public SetupAdmin(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    var admin = await userManager.FindByNameAsync("admin");
                    if (admin == null)
                    {
                        admin = new IdentityUser { UserName = "admin" };
                        var result = await userManager.CreateAsync(admin, "password");
                        if (!result.Succeeded)
                        {
                            throw new DePoolException($"Error Creating admin user {result}");
                        }
                    }

                    if (!await userManager.IsInRoleAsync(admin, "admin"))
                    {
                        var result = await userManager.AddToRoleAsync(admin, "admin");
                        if (!result.Succeeded)
                        {
                            throw new DePoolException($"Error adding user to admin role {result}");
                        }
                    }
                }
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
