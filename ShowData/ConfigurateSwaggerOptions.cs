using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ShowData
{
    /// <summary>
    /// Gets data for swaggr api documentation. Gather xml comments and set them into specific windows with object type depends on models.
    /// </summary>
    public class ConfigurateSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        public ConfigurateSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var field in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(field.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = $"Data Api {field.ApiVersion}",
                    Version = field.ApiVersion.ToString()
                });
            }
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var commentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            options.IncludeXmlComments(commentsFullPath);
        }
    }
}
