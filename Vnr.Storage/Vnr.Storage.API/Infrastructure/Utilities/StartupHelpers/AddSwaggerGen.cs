using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Configuration.Authorization;

namespace Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers
{
    public static partial class StartupHelpers
    {
        public static void AddSwaggerGen(this IServiceCollection services, StorageConfiguration storageConfiguration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(storageConfiguration.ApiVersion, new OpenApiInfo { Title = storageConfiguration.ApiName, Version = storageConfiguration.ApiVersion });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{storageConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{storageConfiguration.IdentityServerBaseUrl}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { storageConfiguration.OidcApiName, storageConfiguration.ApiName },
                            }
                        }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }
    }
}