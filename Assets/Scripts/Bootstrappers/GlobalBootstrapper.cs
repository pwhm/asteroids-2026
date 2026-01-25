using System;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets.Implementation;
using Modules.Gameplay.Implementation;
using Modules.User;
using Modules.User.Implementation;
using Random = UnityEngine.Random;

namespace Bootstrappers
{
    internal static class GlobalBootstrapper
    {
        public static Task AddGlobalServices()
        {
            AssetsModuleInstaller.InstallProjectServices();
            GameplayModuleInstaller.InstallProjectServices();
            ProgressionModuleInstaller.InstallProjectServices();
            
            return Task.CompletedTask;
        }

        public static async Task InitializeGlobalServices()
        {
            Random.InitState( (int)DateTime.UnixEpoch.Ticks);
            
            await GameplayModuleInstaller.InitializeProjectServicesAsync();
            await ProgressionModuleInstaller.InitializeProjectServicesAsync();
        }
    }
}