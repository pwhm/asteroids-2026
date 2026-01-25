using System.Threading.Tasks;
using Modules.Assets.Implementation;

namespace Bootstrappers
{
    internal static class GlobalBootstrapper
    {
        public static async Task AddGlobalServices()
        {
            await AssetsModuleInstaller.InstallProjectServicesAsync();
        }

        public static void InitializeGlobalServices()
        {
            
        }
    }
}