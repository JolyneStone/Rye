using Microsoft.AspNetCore.Builder;
using Rye.Module;

namespace Rye.Web.Module
{
    public abstract class AspNetCoreModule : StartupModule
    {
        public virtual void Configure(IApplicationBuilder app)
        {
            base.Use(app.ApplicationServices);
        }
    }
}
