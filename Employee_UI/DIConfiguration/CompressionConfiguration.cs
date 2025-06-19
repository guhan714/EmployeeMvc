using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace Employee.UI.DIConfiguration;

public static class CompressionConfiguration
{
    public static IServiceCollection AddCompressionConfiguration(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        return services;
    }

    public static IApplicationBuilder UseCompressionConfiguration(this IApplicationBuilder app)
    {
        app.UseResponseCompression();
        return app;
    }
}