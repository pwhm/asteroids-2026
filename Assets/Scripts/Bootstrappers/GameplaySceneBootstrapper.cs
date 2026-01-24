using System.Threading.Tasks;
using Core.Services;
using Modules.Player;
using Modules.Player.Implementation;
using Modules.Wrappables.Implementation;

namespace Bootstrappers
{
    internal sealed class GameplaySceneBootstrapper : SceneBootstrapperBase
    {
        protected override async Task AddSceneServices()
        {
            await WrappablesModuleInstaller.InstallAsync();
            await PlayerModuleInstaller.InstallAsync();
        }

        protected override async Task InitializeScene()
        {
            await WrappablesModuleInstaller.InitializeAsync();
            await PlayerModuleInstaller.Initialize();
            
            Services.GetService<IPlayerService>().SetupForNewRound();
            Services.GetService<IPlayerService>().StartRound();
        }
    }
}