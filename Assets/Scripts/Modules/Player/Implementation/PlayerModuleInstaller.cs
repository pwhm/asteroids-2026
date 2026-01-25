using System;
using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Modules.Player.Implementation
{
    public static class PlayerModuleInstaller 
    {
        public static async Task InstallAsync()
        {
            var key = string.Format(Constants.Addressables.SERVICE_KEY_FORMAT, "player");
            var prefab = await Services.GetService<IAssetService>()
                .LoadPrefab<PlayerService>(key, Constants.Addressables.Tags.GAMEPLAY);

            var service = Object.Instantiate(prefab);
            Services.AddService<IPlayerService>(service, ServiceScope.Scene);
        }

        public static async Task Initialize()
        {
            await Services.GetService<IPlayerService>().InitializeAsync();
        }
    }
}