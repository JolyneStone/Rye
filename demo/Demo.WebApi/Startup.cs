using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.DataAccess;
using Demo.DataAccess.EFCore.DbContexts;
using Demo.WebApi.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Monica;
using Monica.MySql;
using Monica.Web;

using Swashbuckle.AspNetCore.SwaggerGen;

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
                options.DefaultApiVersion = new ApiVersion(1, 1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ErrorResponses = new ErrorResponseProvider();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());


            services
                .AddWebModule(options =>
                {
                    options.GlobalActionFilter.Enabled = true;
                    options.GlobalExceptionFilter.Enabled = true;
                })
                .AddAopModule()
                .AddCacheModule()
                .AddMySqlModule<MyDbConnectionProvider>()
                .AddMySqlEFCodeModule<MyDbContext>("MonicaDemo")
                .AddJwtModule()
                .ConfigureModule();

            services.AddControllers();
        }

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
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
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
