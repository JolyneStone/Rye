using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Raven.CodeGenerator.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raven.CodeGenerator.Abstracts
{
    public abstract class BaseCodeCompiler<TConfig> : ICodeCompiler<TConfig> where TConfig : CodeConfig
    {
        public virtual async Task GenerateAsync(TConfig config)
        {
            var codeDocument = GetDocument();
            var razorResult = GenerateResult(codeDocument);
            var modelEntity = await GetModelEntityAsync(config);
            if (string.IsNullOrEmpty(modelEntity.NameSpace))
            {
                modelEntity.NameSpace = config.NameSpace;
            }

            var (code, namespaces) = SubCodeContent(razorResult.GeneratedCode);

            // 编译C#代码
            var roslynCompiler = new RazorRoslynCompiler();
            var viewInstance = await roslynCompiler.CompileAsync<ModelEntity>(code, razorResult.ClassName, namespaces);

            await viewInstance.ExecuteViewAsync(modelEntity);
            await SaveAsync(config, modelEntity, viewInstance.Stream);
        }

        public abstract Task SaveAsync(TConfig config, ModelEntity modelEntity, Stream stream);

        private (string, string[]) SubCodeContent(string code)
        {
            // 由于我用生成C#代码采取的方式是直接用Roslyn编译脚本，因此需要删去Razor代码中有关命名空间的字符
            var start = code.IndexOf("public");
            var end = code.LastIndexOf("}");
            var cSharpCode = code.Substring(start, end - start);

            // 获取Razor中using命名空间的字符
            var rawCode = code.Substring(0, start);
            var matches = Regex.Matches(rawCode, @"using[\b\s](?<namespace>[\w\.]+?);");
            var namespaces = new List<string>();
            foreach (Match match in matches)
            {
                var namespaceStr = match.Groups["namespace"]?.Value;
                if (!String.IsNullOrWhiteSpace(namespaceStr))
                {
                    namespaces.Add(namespaceStr);
                }
            }

            return (cSharpCode, namespaces.ToArray());
        }

        protected abstract Task<ModelEntity> GetModelEntityAsync(TConfig config);

        protected abstract RazorCodeDocument GetDocument();

        protected virtual RazorPageGeneratorResult GenerateResult(RazorCodeDocument codeDocument)
        {
            if (codeDocument is null)
            {
                throw new ArgumentNullException(nameof(codeDocument));
            }

            codeDocument.SetImportSyntaxTrees(new[] { RazorSyntaxTree.Parse(RazorSourceDocument.Create(@"
                @using System
                @using System.Threading.Tasks
                @using System.Collections.Generic
                @using System.Collections
                ", fileName: null)) });
            var cSharpDocument = codeDocument.GetCSharpDocument();

            if (cSharpDocument.Diagnostics.Any())
            {
                var diagnostics = string.Join(Environment.NewLine, cSharpDocument.Diagnostics);
                throw new InvalidOperationException($"无法生成Razor页面代码，一个或多个错误发生:{diagnostics}");
            }

            return new RazorPageGeneratorResult
            {
                ClassName = "TempleteRazorPageView",
                GeneratedCode = cSharpDocument.GeneratedCode,
            };
        }
    }
}
