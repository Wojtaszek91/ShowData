using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShowData.Data;
using ShowData.Repository;
using ShowData.Repository.IRepository;
using AutoMapper;
using ShowData.Mappings;
using System.Reflection;
using System.IO;

namespace ShowData
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<IShowModelRepository, ShowModelRepository>();
            services.AddScoped<IDataOverviewRepository, DataOverviewRepository>();
            services.AddAutoMapper(typeof(ShowMapper));
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("ShowDataApiSpec",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Show data API",
                        Version = "1.0",
                        Description = "CRUD methods for ShowData and DataOverview models",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Michal Wojtaszek",
                            Email = "wojmichal91@gmail.com",
                            Url = new Uri("http://www.google.pl")
                        }
                    });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(commentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ShowDataApiSpec/swagger.json", "ShowModel DataOverview Api");
             //   options.SwaggerEndpoint("/swagger/DataOverviewApiSpec/swagger.json", "Data overview Api");
                options.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
