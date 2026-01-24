using System;
using System.Threading.Tasks;
using Core.Services;
using Modules.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Modules.Wrappables.Implementation
{
    public static class WrappablesModuleInstaller
    {
        public static async Task InstallAsync()
        {
            var key = string.Format(Constants.AddressablesFormats.SERVICE, "wrappables");
            var handle = Addressables.LoadAssetAsync<GameObject>(key);
            
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception(handle.OperationException.Message);
            }
            
            var service = Object.Instantiate(handle.Result).GetComponent<IWrappablesService>();
            Services.AddService<IWrappablesService>(service, ServiceScope.Scene);
        }

        public static async Task InitializeAsync()
        {
            await Services.GetService<IWrappablesService>().InitializeAsync();
        }
    }
}