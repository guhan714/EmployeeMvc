using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Employee.UI.DIConfiguration
{
    public static class RateLimiterConfig
    {
        public static IServiceCollection AddRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 5;
                    options.Window = TimeSpan.FromMinutes(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 0;
                });
                rateLimiterOptions.RejectionStatusCode = 529;
            });
            return services;
        }


        public static IApplicationBuilder UseRateLimiters(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
            return app;
        }
    }
}
