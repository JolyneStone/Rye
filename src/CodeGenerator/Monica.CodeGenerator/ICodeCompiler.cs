using System.Threading.Tasks;

namespace Monica.CodeGenerator
{
    public interface ICodeCompiler<TConfig> where TConfig : CodeConfig
    {
        Task GenerateAsync(TConfig config);
    }
}
