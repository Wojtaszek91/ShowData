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
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ShowData
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ItaskRepository, taskRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(typeof(ShowMapper));

            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat ="'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigurateSwaggerOptions>();
            services.AddSwaggerGen();

            var appSettingsClass = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsClass.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.Configure<AppSettings>(appSettingsClass);
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, 
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(a =>
            {
                a.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach(var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToLowerInvariant());
                }
                options.RoutePrefix = "";
            });
            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ShowDataApiSpec/swagger.json", "task DataOverview Api");
            //    options.SwaggerEndpoint("/swagger/DataOverviewApiSpec/swagger.json", "Data overview Api");
            //    options.RoutePrefix = "";
            //});
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            // use before Authorization !
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
