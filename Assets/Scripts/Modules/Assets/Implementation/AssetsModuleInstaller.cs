using System.Threading.Tasks;
using Core.Services;

namespace Modules.Assets.Implementation
{
    public static class AssetsModuleInstaller
    {
        public static async Task InstallProjectServicesAsync()
        {
            var assetService = new AssetService();
            
            Services.AddService<IAssetService>(assetService, ServiceScope.Global);
        }
    }
}