using System.Threading.Tasks;
using Modules.Wrappables.Implementation;

namespace Bootstrappers
{
    internal sealed class GameplaySceneBootstrapper : SceneBootstrapperBase
    {
        protected override async Task AddSceneServices()
        {
            await WrappablesModuleInstaller.InstallAsync();
        }

        protected override async Task InitializeScene()
        {
            await WrappablesModuleInstaller.Initialize();
        }
    }
}