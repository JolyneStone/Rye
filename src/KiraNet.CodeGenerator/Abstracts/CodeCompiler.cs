using System.Threading.Tasks;

namespace KiraNet.CodeGenerator.Abstracts
{
    public interface ICodeCompiler<TConfig> where TConfig : CodeConfig
    {
        Task<RazorPageGeneratorResult> GenerateAsync(TConfig config);
    }
}
