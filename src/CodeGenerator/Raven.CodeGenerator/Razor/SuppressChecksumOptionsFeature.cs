using Microsoft.AspNetCore.Razor.Language;

using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.CodeGenerator.Razor
{
    internal class SuppressChecksumOptionsFeature : RazorEngineFeatureBase, IConfigureRazorCodeGenerationOptionsFeature
    {
        public int Order { get; set; }

        public void Configure(RazorCodeGenerationOptionsBuilder options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.SuppressChecksum = true;
        }
    }
}
