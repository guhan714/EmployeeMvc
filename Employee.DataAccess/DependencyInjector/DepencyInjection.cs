using Employee.BusinessLogic.Interfaces.IRepository;
using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.BusinessLogic.Interfaces.Service.Security;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.DataAccess.Persistence.Repository.Domain;
using Employee.DataAccess.Persistence.Repository.Masters;
using Employee.DataAccess.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employee.DataAccess.DependencyInjector
{
    public static class DepencyInjection
    {
            
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EmployeeConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString!);
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentMaster, DepartmentMaster>();
            services.AddScoped<IQueryService, EmployeeQueryService>();
            services.AddScoped<ICryptographyService, BcryptHashingService>();
            return services;
        }
    }
}
