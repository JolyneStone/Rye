﻿using Microsoft.Extensions.DependencyInjection;
using Rye.Cache;
using Rye.Enums;
using Rye.Module;
using Rye.Web.Options;
using System;

namespace Rye.Web.Module
{
    [DependsOnModules(typeof(CacheModule))]
    public class AspNetCoreConfigModule: AspNetCoreModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 1;

        private Action<RyeWebOptions> _action;

        public AspNetCoreConfigModule(Action<RyeWebOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddWebRye(_action);
        }
    }
}
