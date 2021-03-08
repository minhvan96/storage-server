using Microsoft.Extensions.DependencyInjection;
using Vnr.Storage.API.Configuration;

namespace Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers
{
    public static partial class StartupHelpers
    {
        public static IServiceCollection AddApiCors(this IServiceCollection services, StorageConfiguration storageConfiguration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (storageConfiguration.CorsAllowAnyOrigin)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(storageConfiguration.CorsAllowOrigins);
                        }

                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}