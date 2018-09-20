using FilterLists.Api.DependencyInjection.Extensions;
using FilterLists.Data;
using FilterLists.Data.Seed.Extensions;
using FilterLists.Services.DependencyInjection.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FilterLists.Api
{
    [UsedImplicitly]
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFilterListsApiServices(Configuration);
            services.AddFilterListsApi();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                opts =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                        opts.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
            MigrateAndSeedDatabase(app);
        }

        private void MigrateAndSeedDatabase(IApplicationBuilder app)
        {
            var dataPath = Configuration.GetSection("DataDirectory").GetValue<string>("Path");
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var filterListsDbContext = serviceScope.ServiceProvider.GetService<FilterListsDbContext>();
                filterListsDbContext.Database.Migrate();
                filterListsDbContext.SeedOrUpdate(dataPath);
            }
        }
    }
}