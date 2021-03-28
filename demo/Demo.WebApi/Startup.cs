using Demo.Core.Common;
using Demo.Core.Common.Enums;
using Demo.DataAccess;
using Demo.DataAccess.EFCore.DbContexts;
using Demo.Library;
using Demo.WebApi.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Rye;
using Rye.MySql;
using Rye.Web;

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
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
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

            services.AddCors(option => option.AddPolicy("cors", policy =>
            {
                policy.AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowCredentials();
            }));

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            services = services.UseDynamicProxyService();
            services.AddMySqlPersmission<int>();
            services
                .AddCoreModule(options =>
                {
                    options.AutoInjection = true;
                })
                .AddWebModule(options =>
                {
                    options.GlobalActionFilter.Enabled = true;
                    options.GlobalExceptionFilter.Enabled = true;
                })
                .AddBusinessModule(options =>
                {
                    options.VerfiyCodeExpire = 5 * 60;
                })
                .AddRedisCacheModule()
                .AddMySqlModule<MyDbConnectionProvider>()
                .AddMySqlEFCodeModule(builder =>
                {
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye.GetDescription());
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye_Read.GetDescription());
                })
                .AddJwtModule()
                .AddAuthorizationModule<int>()
                .AddModule<DemoModule>()
                .ConfigureModule();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseCors("cors");
            app.UseRouting();
            app.UseSecurity();
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
                return new JsonResult(new ApiResult
                {
                    Code = (int)CommonStatusCode.Fail,
                    Message = "Unsupported Api Version",
                });
            }
        }
    }
}
