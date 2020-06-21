using System.Threading.Tasks;

namespace Raven.CodeGenerator.Abstracts
{
    public interface ICodeCompiler<TConfig> where TConfig : CodeConfig
    {
        Task<RazorPageGeneratorResult> GenerateAsync(TConfig config);
    }
}
