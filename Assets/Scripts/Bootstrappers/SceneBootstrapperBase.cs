using System.Threading.Tasks;
using Core.Services;
using UnityEngine;

namespace Bootstrappers
{
    internal abstract class SceneBootstrapperBase : MonoBehaviour
    {
        private bool _coldStart;
        
        private void Awake()
        {
            if (Services.IsStartingCold)
            {
                Debug.Log($"#Bootstrapper# {GetType().Name} detected cold start");
                
                _coldStart = true;
                GlobalBootstrapper.AddGlobalServices();
            }
                
            AddSceneServices();
        }

        private void Start()
        {
            if (_coldStart)
            {
                GlobalBootstrapper.InitializeGlobalServices();
            }
            
            InitializeScene();
        }

        protected abstract Task AddSceneServices();
        protected abstract Task InitializeScene();
    }
}