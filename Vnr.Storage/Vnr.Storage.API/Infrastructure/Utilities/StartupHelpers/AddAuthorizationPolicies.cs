using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Configuration.Contants;

namespace Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers
{
    public static partial class StartupHelpres
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services, StorageConfiguration storageConfiguration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy =>
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                                (c.Type == JwtClaimTypes.Role && c.Value == storageConfiguration.AdministrationRole) ||
                                (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == storageConfiguration.AdministrationRole)
                            )
                        ));

                options.AddPolicy("ClientAccess", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "Vnr-StorageServer");
                });
            });
        }
    }
}