using System;
using System.IO;
using System.Reflection;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace FilterLists.Api.DependencyInjection.Extensions
{
    public static class ConfigureServicesCollection
    {
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        public static void AddFilterListsApi(this IServiceCollection services)
        {
            services.ConfigureCookiePolicy();
            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddMvcCustom();
            services.AddVersionedApiExplorer(
                opts =>
                {
                    opts.GroupNameFormat = "'v'VVV";
                    opts.SubstituteApiVersionInUrl = true;
                });
            services.AddApiVersioning(opts => opts.ReportApiVersions = true);
            services.AddRoutingCustom();
            services.AddSwaggerGenCustom();
            TelemetryDebugWriter.IsTracingDisabled = true;
        }

        private static void ConfigureCookiePolicy(this IServiceCollection services) =>
            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.CheckConsentNeeded = context => true;
                opts.MinimumSameSitePolicy = SameSiteMode.None;
            });

        private static void AddMvcCustom(this IServiceCollection services) =>
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        private static void AddRoutingCustom(this IServiceCollection services) =>
            services.AddRouting(opts => opts.LowercaseUrls = true);

        private static void AddSwaggerGenCustom(this IServiceCollection services) =>
            services.AddSwaggerGen(opts =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                    opts.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                opts.OperationFilter<SwaggerDefaultValues>();
                opts.IncludeXmlComments(XmlCommentsFilePath);
            });

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = "FilterLists API",
                Version = description.ApiVersion.ToString(),
                Description =
                    "A REST-ish API for FilterLists, the independent, comprehensive directory of all public filter and hosts lists for advertisements, trackers, malware, and annoyances." +
                    Environment.NewLine +
                    " - {version} has to be specified manually (to \"1\") in Swagger playground below due to https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/370",
                Contact = new Contact
                    {Url = "https://github.com/collinbarrett/FilterLists/", Name = "FilterLists - GitHub"},
                License = new License
                    {Name = "MIT License", Url = "https://github.com/collinbarrett/FilterLists/blob/master/LICENSE"}
            };

            if (description.IsDeprecated)
                info.Description += " This API version has been deprecated.";
            return info;
        }
    }
}