using bitirmeProje.Domain.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Tracing;

namespace bitirmeProje.Configuration
{
    public static class DataBaseStartup
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
        {
            string connectionString = "Server=analizgaraj.com\\MSSQLSERVER2019;Database=analizga_pollify;User Id=analizga_pollify;Password=tq0m5E7~1;TrustServerCertificate=True;";
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
            return services;
        }
    }
}
