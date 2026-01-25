using System.Threading.Tasks;
using Core.Services;

namespace Modules.Gameplay.Implementation
{
    public static class GameplayModuleInstaller
    {
        public static void InstallProjectServices()
        {
            Services.AddService<IGameplayService>(new  GameplayService(), ServiceScope.Global);
        }

        public static async Task InitializeProjectServicesAsync()
        {
            await Services.GetService<IGameplayService>().InitializeAsync();
        }
    }
}