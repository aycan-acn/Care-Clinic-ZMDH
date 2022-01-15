using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Wdpr_Groep_E.Data;
using Wdpr_Groep_E.Models;

namespace Wdpr_Groep_E
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var getService = scope.ServiceProvider;
                try
                {
                    var getContext = getService.GetRequiredService<WdprContext>();
                    var getUserManager = getService.GetRequiredService<UserManager<AppUser>>();
                    var getRoleManager = getService.GetRequiredService<RoleManager<IdentityRole>>();
                    await SeedContext.CreateRolesAsync(getUserManager, getRoleManager);
                    await SeedContext.CreateModeratorAsync(getUserManager, getRoleManager);
                    await SeedContext.CreateTestUserAsync(getUserManager, getRoleManager);
                    await SeedContext.CreateOrthopedagogen(getUserManager, getRoleManager);
                }
                catch
                {
                    System.Console.WriteLine("Het seeding van database is niet gelukt");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
