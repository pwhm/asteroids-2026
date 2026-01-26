using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using Object = UnityEngine.Object;

namespace Modules.Wrappables.Implementation
{
    public static class WrappablesModuleInstaller
    {
        public static async Task InstallAsync()
        {
            var key = string.Format(Constants.Addressables.SERVICE_KEY_FORMAT, "wrappables");
            var prefab = await Services.GetService<IAssetService>()
                .LoadPrefab<WrappablesService>(key, Constants.Addressables.Tags.GAMEPLAY);

            var service = Object.Instantiate(prefab);
            Services.AddService<IWrappablesService>(service, ServiceScope.Scene);
        }

        public static async Task InitializeAsync()
        {
            await Services.GetService<IWrappablesService>().InitializeAsync();
        }
    }
}