using bitirmeProje.Domain.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Tracing;

namespace bitirmeProje.Configuration
{
    public static class DataBaseStartup
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
        {
            string connectionString = "Server=DESKTOP-F7K2U1C\\SQLEXPRESS;database=AnketSistemi;Integrated Security = true; TrustServerCertificate=True;";
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
            return services;
        }
    }
}
