using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.DataAccess;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Monica;
using Monica.MySql;
using Monica.Web;

namespace Demo.WebApi
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
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ErrorResponses = new ErrorResponseProvider();
            });

            services
                .AddWebModule()
                .AddAopModule()
                .AddCacheModule()
                .AddMySqlModule<MyDbConnectionProvider>()
                .AddMySqlEFCodeModule()
                .AddJwtModule()
                .ConfigureModule();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.UseApiVersioning();

            app.UseModule();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class ErrorResponseProvider : IErrorResponseProvider
        {
            public IActionResult CreateResponse(ErrorResponseContext context)
            {
                return new JsonResult(new Result
                {
                    Code = (int)StatusCode.Fail,
                    Message = "Unsupported Api Version",
                });
            }
        }
    }
}
