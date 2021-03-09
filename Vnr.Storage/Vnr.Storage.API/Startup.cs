using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Converters;
using System.IO;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Configuration.Contants;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Utilities.StartupHelpers;

namespace Vnr.Storage.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddHttpContextAccessor();
            services.AddDirectoryBrowser();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddDbContext<StorageContext>(options =>
                       options.UseSqlite(
                           Configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(typeof(Startup));

            var storageConfiguration = Configuration.GetSection(nameof(StorageConfiguration)).Get<StorageConfiguration>();
            services.AddSingleton(storageConfiguration);
            services.AddApiCors(storageConfiguration);

            services.AddApiAuthentication(storageConfiguration);
            services.AddAuthorizationPolicies(storageConfiguration);
            services.AddSwaggerGen(storageConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StorageConfiguration storageConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(storageConfiguration);
            }

            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
            app.UseStaticFiles();
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, ArchiveConstants.PhysicalFileProviderPath)),
                RequestPath = ArchiveConstants.ArchiveRequestPath,
                EnableDirectoryBrowsing = true,
            });

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}