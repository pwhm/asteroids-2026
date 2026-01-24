using System;
using System.Threading.Tasks;
using Core.Services;
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
            var key = string.Format(Constants.AddressablesFormats.SERVICE, "player");
            var handle = Addressables.LoadAssetAsync<GameObject>(key);
            
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception(handle.OperationException.Message);
            }
            
            var service = Object.Instantiate(handle.Result).GetComponent<IPlayerService>();
            Services.AddService<IPlayerService>(service, ServiceScope.Scene);
        }

        public static async Task Initialize()
        {
            await Services.GetService<IPlayerService>().InitializeAsync();
        }
    }
}