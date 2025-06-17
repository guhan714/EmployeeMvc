using Employee.DataAccess.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Employee.UI.DIConfiguration
{
    public  static class DbcontextConfiguration
    {
        public static IServiceCollection AddDbContextConfiguraion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options
                =>
            {
                options.UseSqlServer(configuration.GetConnectionString("EmployeeConnection"));
            });
            return services;
        }
    }
}
