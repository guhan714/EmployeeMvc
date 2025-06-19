using Employee.BusinessLogic.Interfaces.Service.Domain;
using Employee.BusinessLogic.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Employee.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILoginService, LoginService>();
            return services;
        }
    }
}
