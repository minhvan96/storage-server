using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Vnr.Storage.API.Configuration;

namespace Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers
{
    public static partial class StartupHelpres
    {
        public static void AddApiAuthentication(this IServiceCollection services, StorageConfiguration storageConfiguration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.BackchannelHttpHandler = storageConfiguration.JwtBackChannelHandler;
                options.Audience = "Vnr-StorageServer-API";
                options.Authority = storageConfiguration.IdentityServerBaseUrl;
                options.RequireHttpsMetadata = storageConfiguration.RequireHttpsMetadata;

                // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1214
                if (options.SecurityTokenValidators.FirstOrDefault() is JwtSecurityTokenHandler jwtSecurityTokenHandler)
                    jwtSecurityTokenHandler.MapInboundClaims = false;

                options.SaveToken = true;
            });
        }
    }
}