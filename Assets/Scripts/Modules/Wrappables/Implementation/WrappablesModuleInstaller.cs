using System;
using System.Threading.Tasks;
using Core.Services;
using Modules.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            
            var service = handle.Result.GetComponent<IWrappablesService>();
            Services.AddService<IWrappablesService>(service, ServiceScope.Scene);
        }

        public static async Task Initialize()
        {
            await Services.GetService<IWrappablesService>().InitializeAsync();
        }
    }
}