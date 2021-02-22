using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Rye.CodeGenerator.Razor
{
    public abstract class RoslynCompiler
    {
        protected static readonly IList<MetadataReference> _applicationReferences = new List<MetadataReference>();
        public abstract Task<RazorPageViewBase<TModel>> CompileAsync<TModel>(string code, string className, string[] namespaces);

        static RoslynCompiler()
        {
            InitApplicationReferences();
            //var path = typeof(RoslynCompiler).Assembly.Location;
            //using (var stream = File.OpenRead(path))
            //{
            //    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
            //    var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
            //    _applicationReferences.Add(assemblyMetadata.GetReference(filePath: path));
            //}

            //_applicationReferences.Add(CreateMetadataFileReference(typeof(object).Assembly.Location));
        }

        private static void InitApplicationReferences()
        {
            if (_applicationReferences != null && _applicationReferences.Any())
            {
                return;
            }

            var metadataReferences = new List<MetadataReference>();

            //string runtimePath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            //var mscorlibFile = Path.Combine(runtimePath, "mscorlib.dll");
            //if (File.Exists(mscorlibFile))
            //    metadataReferences.Add(CreateMetadataFileReference(mscorlibFile));
            //else
            //    metadataReferences.Add(CreateMetadataFileReference(Path.Combine(runtimePath, "mscorlib.ni.dll")));

            //metadataReferences.Add(CreateMetadataFileReference(typeof(object).Assembly.Location));
            //metadataReferences.Add(CreateMetadataFileReference(typeof(DynamicObject).Assembly.Location));
            //metadataReferences.Add(CreateMetadataFileReference(entryAssembly.Location));

            //var refAssemblies = assemblyLocator.DependencyAssemblies();//.ReferenceAssemblies();
            //var razorAssembly = typeof(RoslynCompiler).GetTypeInfo().Assembly;
            //var razorAssemblyName = razorAssembly.GetName().FullName;
            //if (refAssemblies.FirstOrDefault(x => String.Equals(
            //    x.GetName().FullName, 
            //    razorAssemblyName, 
            //    StringComparison.InvariantCultureIgnoreCase)) == null)
            //{
            //    metadataReferences.Add(CreateMetadataFileReference(razorAssembly.Location));
            //}
            //var refAssemblies = assemblyLocator.DependencyAssemblies();

            //foreach (var refassembly in refAssemblies)
            //{
            //    metadataReferences.Add(CreateMetadataFileReference(refassembly.Location));
            //}

            //var refassemblyNames = refAssemblies.Select(x => x.GetName());
            var entryAssembly = Assembly.GetEntryAssembly();
            var refMvcAssemblyNames = typeof(RoslynCompiler).Assembly.GetReferencedAssemblies();
            var refAssembies = entryAssembly.GetReferencedAssemblies().Except(refMvcAssemblyNames, new AssemblyNameCompare());
            refAssembies = refAssembies.Union(DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable).Select(lib => new AssemblyName(lib.Name)), new AssemblyNameCompare());
            foreach (var assemblyName in refAssembies)
            {
                var assembly = Assembly.Load(assemblyName);
                _applicationReferences.Add(CreateMetadataFileReference(assembly.Location));
            }

            _applicationReferences.Add(CreateMetadataFileReference(entryAssembly.Location));
        }

        private static MetadataReference CreateMetadataFileReference(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                return assemblyMetadata.GetReference(filePath: path);
            }
        }

        private class AssemblyNameCompare : IEqualityComparer<AssemblyName>
        {
            public bool Equals(AssemblyName x, AssemblyName y)
            {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(AssemblyName obj)
            {
                return obj.FullName.GetHashCode();
            }
        }
    }
}
