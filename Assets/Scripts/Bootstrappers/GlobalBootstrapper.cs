using System;
using System.Threading.Tasks;
using Modules.Assets.Implementation;
using Modules.Gameplay.Implementation;
using Random = UnityEngine.Random;

namespace Bootstrappers
{
    internal static class GlobalBootstrapper
    {
        public static Task AddGlobalServices()
        {
            AssetsModuleInstaller.InstallProjectServices();
            GameplayModuleInstaller.InstallProjectServices();
            
            return Task.CompletedTask;
        }

        public static void InitializeGlobalServices()
        {
            Random.InitState( (int)DateTime.UnixEpoch.Ticks);
        }
    }
}