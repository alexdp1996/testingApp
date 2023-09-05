using Data.Repositories;
using Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddDbContext<Context>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DatabaseConnectionString")));
        services.AddTransient<ICustomerRepository, CustomerRepository>();
    })
    .Build();

host.Run();
