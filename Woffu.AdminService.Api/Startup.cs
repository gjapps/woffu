using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using Woffu.AdminService.Api.Clients;
using Woffu.AdminService.Api.Interfaces;

namespace Woffu.AdminService.Api
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
            services.AddControllersWithViews();
            services.AddControllers();

            services.AddApiVersioning(o => {});

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null); ;
            services.AddAuthorization();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.CURRENT_VERSION, new OpenApiInfo { Title = Constants.SERVICE_NAME, Version = Constants.CURRENT_VERSION });
            });

            services.AddHttpClient<IJobTitlesServiceClient, JobTitlesServiceClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>(Constants.JOB_TITLE_SERVICE_BASE_URL));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(a => a.Run(async context => {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;

                if (exception is HttpRequestException)
                {
                    HttpStatusCode code;
                    if (Enum.TryParse<HttpStatusCode>(exception.Message, out code))
                    {
                        context.Response.StatusCode = (int)code;
                    }
                }

                var result = JsonConvert.SerializeObject(exception.Message);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(result);
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(o => {
                o.RouteTemplate = $"{Constants.API_BASE_URL}swagger/{{documentName}}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint($"/{Constants.API_BASE_URL}swagger/{Constants.CURRENT_VERSION}/swagger.json", $"{Constants.SERVICE_NAME} {Constants.CURRENT_VERSION}");
                c.RoutePrefix = $"{Constants.API_BASE_URL}swagger";
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
