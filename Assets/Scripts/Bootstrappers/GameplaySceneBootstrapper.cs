using System.Threading.Tasks;
using Core.Services;
using Modules.Asteroids;
using Modules.Asteroids.Implementation;
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
            await AsteroidsModuleInstaller.InstallAsync();
        }

        protected override async Task InitializeScene()
        {
            await WrappablesModuleInstaller.InitializeAsync();
            await PlayerModuleInstaller.Initialize();
            await AsteroidsModuleInstaller.InitializeAsync();
            
            
            // Move to gameplay service later
            Services.GetService<IPlayerService>().SetupForNewRound();
            Services.GetService<IPlayerService>().StartRound();
            
            Services.GetService<IAsteroidsService>().SetupForNewRound();
            Services.GetService<IAsteroidsService>().StartRound();
        }
    }
}