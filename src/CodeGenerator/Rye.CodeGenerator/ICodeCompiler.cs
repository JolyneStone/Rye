using System.Threading.Tasks;

namespace Rye.CodeGenerator
{
    public interface ICodeCompiler<TConfig> where TConfig : CodeConfig
    {
        Task GenerateAsync(TConfig config);
    }
}
