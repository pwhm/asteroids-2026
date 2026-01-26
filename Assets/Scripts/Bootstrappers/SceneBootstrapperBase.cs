using System.Threading.Tasks;
using Core.Services;
using Modules.Assets;
using Modules.Common;
using UnityEngine;

namespace Bootstrappers
{
    public abstract class SceneBootstrapperBase : MonoBehaviour
    {
        private async void Awake()
        {
            var coldStart = false;
            if (Services.IsStartingCold)
            {
                Debug.Log($"#Bootstrapper# {GetType().Name} detected cold start");
                
                coldStart = true;
                await GlobalBootstrapper.AddGlobalServices();
            }
                
            await AddSceneServices();
            
            if (coldStart)
            {
                await GlobalBootstrapper.InitializeGlobalServices();
            }
            
            await InitializeScene();
        }

        private void OnDestroy()
        {
            Services.GetService<IAssetService>().ReleaseAssets(Constants.Addressables.Tags.GAMEPLAY);
            Services.RemoveSceneServices();
        }

        protected abstract Task AddSceneServices();
        protected abstract Task InitializeScene();
    }
}