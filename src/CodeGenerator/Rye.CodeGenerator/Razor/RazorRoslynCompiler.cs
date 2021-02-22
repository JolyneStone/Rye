using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.CodeGenerator.Razor
{
    /// <summary>
    /// 利用Roslyn进行动态编译
    /// </summary>
    public class RazorRoslynCompiler : RoslynCompiler
    {
        //private static Assembly _rootAssembly = typeof(RoslynCompiler).GetTypeInfo().Assembly;

        //public EmitResult CompileResult { get; private set; }
        //public RazorRoslynCompiler(string layoutName):base()
        //{
        //    if (string.IsNullOrWhiteSpace(layoutName))
        //    {
        //        throw new ArgumentException(nameof(layoutName));
        //    }

        //    _viewClassName = layoutName;
        //}


        /// <summary>
        /// 编译Razor代码，最终生成一个代表Razor的类
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public override async Task<RazorPageViewBase<TModel>> CompileAsync<TModel>(string code, string className, string[] namespaces)
        {
            // 被我所注释的代码参考自 https://github.com/maxzhang1985/YOYOFx/blob/Dev/AspNetCore/YOYO.AspNetCore.ViewEngine.Razor/RoslynCompileService.cs
            // 但在编译过程中，调用CSharpCompilation.Create方法时，需要引入netstandard2.0的程序集
            // 而Object所在的程序集在编写代码阶段按F12查看是在netstandard2.0中
            // 但由于GutsMVC调用方的不同可能为netcore或netframework等程序集
            // 而为了避免硬编码，决定采用另一种方法

            //var assemblyName = Path.GetRandomFileName();

            //var sourceText = SourceText.From(code, Encoding.UTF8);
            //var syntaxTree = CSharpSyntaxTree.ParseText(
            //    sourceText,
            //    new CSharpParseOptions(),
            //    assemblyName);

            ////var metadata = MetadataReference.CreateFromFile(this.GetType().Assembly.Location);
            ////var metadata = MetadataReference.CreateFromFile(typeof(Object).Assembly.Location);
            ////_applicationReferences.Add(metadata);
            //var net = Assembly.Load(new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"));
            //var netReference = MetadataReference.CreateFromFile(net.Location);
            //if(null==_applicationReferences.FirstOrDefault(x=>x.Display == netReference.Display))
            //{
            //    _applicationReferences.Add(netReference);
            //}

            //var compilation = CSharpCompilation.Create(
            //    assemblyName,
            //    new[] { syntaxTree },
            //    _applicationReferences,
            //    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            //using (var assemblyStream = new MemoryStream())
            //{
            //    CompileResult = compilation.Emit(
            //        assemblyStream,
            //        options: new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb));


            //    // 编译不成功
            //    if (!CompileResult.Success)
            //    {
            //        //var options = ScriptOptions.Default.AddReferences
            //        //if (!compilation.References.Any() && !_applicationReferences.Any())
            //        //{
            //        //    throw new InvalidOperationException("编译Razor视图无法成功");
            //        //}

            //        return null;
            //    }

            //var templateType = LoadTypeForAssemblyStream(assemblyStream);
            //return templateType;
            //}
            var defaultNamespaces = new List<string> { "System", "System.Threading.Tasks", "Rye.CodeGenerator", "Rye.CodeGenerator.Razor" };


            if (namespaces != null && namespaces.Length > 0)
            {
                defaultNamespaces.AddRange(namespaces.Except(namespaces));
            }

            var options = ScriptOptions.Default
                .AddImports(defaultNamespaces)
                .AddReferences(_applicationReferences);


            var result = CSharpScript.Create(code, options)
                .ContinueWith($"new {className}()");
            try
            {
                var value = (await result.RunAsync()).ReturnValue;
                var view = value as RazorPageViewBase<TModel>;
                return view;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //private Type LoadTypeForAssemblyStream(MemoryStream assemblyStream)
        //{
        //    assemblyStream.Seek(0, SeekOrigin.Begin);
        //    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(assemblyStream);
        //    var type = assembly.GetTypes().FirstOrDefault();
        //    return type;
        //}
    }
}
