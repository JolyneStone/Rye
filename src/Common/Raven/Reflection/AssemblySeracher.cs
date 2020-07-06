using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Raven.Reflection
{
    public class AssemblySeracher : SearcherBase<Assembly>
    {
        protected override List<Assembly> FindAllItems()
        {
            List<Assembly> list = new List<Assembly>();
            var libs = DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable);
            foreach (var lib in libs)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                list.Add(assembly);
            }
            return list;
        }
    }
}
