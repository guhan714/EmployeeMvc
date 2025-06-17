namespace Employee.UI.DIConfiguration
{
    public static class SessionConfiguration
    {
        public static IServiceCollection AddSessionConfiguration(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Session timeout
                options.Cookie.HttpOnly = true; // Prevents JavaScript access
                options.Cookie.IsEssential = true; // Required for GDPR compliance (General Data Protection Regulation)
            });
            return services;
        }


        public static IApplicationBuilder UseSessionconfiguration(this IApplicationBuilder app)
        {
            app.UseSession();
            return app;
        }
    }
}
