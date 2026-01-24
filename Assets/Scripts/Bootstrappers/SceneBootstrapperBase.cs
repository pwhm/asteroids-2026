using System.Threading.Tasks;
using Core.Services;
using UnityEngine;

namespace Bootstrappers
{
    internal abstract class SceneBootstrapperBase : MonoBehaviour
    {
        private async void Awake()
        {
            var coldStart = false;
            if (Services.IsStartingCold)
            {
                Debug.Log($"#Bootstrapper# {GetType().Name} detected cold start");
                
                coldStart = true;
                GlobalBootstrapper.AddGlobalServices();
            }
                
            await AddSceneServices();
            
            if (coldStart)
            {
                GlobalBootstrapper.InitializeGlobalServices();
            }
            
            await InitializeScene();
        }
        
        protected abstract Task AddSceneServices();
        protected abstract Task InitializeScene();
    }
}