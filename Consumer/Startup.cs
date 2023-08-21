using Data;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Consumer.Startup))]

namespace Consumer
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.GetContext().ApplicationRootPath)
                .AddJsonFile($"local.settings.json", optional: false)
                .Build();

            var dbSetting = configuration.GetSection("Database").Get<DatabaseSettings>();
            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(dbSetting.ConnectionString));
            builder.Services.AddTransient<IRepository<Order>, IRepository<Order>>();
            builder.Services.AddTransient<IRepository<Customer>, IRepository<Customer>>();
        }
    }
}
