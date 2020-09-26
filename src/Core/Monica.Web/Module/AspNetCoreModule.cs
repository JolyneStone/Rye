using Microsoft.AspNetCore.Builder;
using Monica.Module;

namespace Monica.Web.Module
{
    public abstract class AspNetCoreModule : StartupModule
    {
        public virtual void Configure(IApplicationBuilder app)
        {
            base.Configure(app.ApplicationServices);
        }
    }
}
