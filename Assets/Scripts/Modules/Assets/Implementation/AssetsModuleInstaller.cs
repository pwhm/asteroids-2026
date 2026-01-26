using Core.Services;

namespace Modules.Assets.Implementation
{
    public static class AssetsModuleInstaller
    {
        public static void InstallProjectServices()
        {
            var assetService = new AssetService();
            
            Services.AddService<IAssetService>(assetService, ServiceScope.Global);
        }
    }
}