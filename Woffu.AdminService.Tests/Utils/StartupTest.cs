using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Woffu.AdminService.Api;

namespace Woffu.AdminService.Tests.Utils
{
    public class StartupTest : StartupBase
    {
        public override void Configure(IApplicationBuilder app)
        {
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();

            });

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddControllers().AddApplicationPart(Assembly.Load("Woffu.AdminService.Api"));

            services.AddApiVersioning(o => { });

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null); ;
            services.AddAuthorization();
        }
    }
}
