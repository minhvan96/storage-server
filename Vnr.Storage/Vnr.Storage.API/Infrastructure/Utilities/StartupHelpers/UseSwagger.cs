using Microsoft.AspNetCore.Builder;
using Vnr.Storage.API.Configuration;

namespace Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers
{
    public static partial class StartupHelpers
    {
        public static void UseSwagger(this IApplicationBuilder app, StorageConfiguration storageConfiguration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{storageConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", storageConfiguration.ApiName);

                c.OAuthClientId(storageConfiguration.OidcSwaggerUIClientId);
                c.OAuthAppName(storageConfiguration.ApiName);
                c.OAuthUsePkce();
            });
        }
    }
}