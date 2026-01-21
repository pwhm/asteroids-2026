using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Core.Scene
{
    public static class SceneManager
    {
        private const string SCENE_KEY_FORMAT = "scene/{0}";
        
        private static AsyncOperationHandle<SceneInstance>? _currentSceneHandle = null;
        
        public static async Task SwitchTo(string sceneName)
        {
            await TryUnloadingCurrentScene();
            
            var key =  string.Format(SCENE_KEY_FORMAT, sceneName);
            var handle = Addressables.LoadSceneAsync(key);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // Call on scene loaded event?
                return;
            }
            
            throw new Exception($"#SceneManager# Failed to switch scene to {key}: {handle.OperationException}");
        }

        private static async Task TryUnloadingCurrentScene()
        {
            if (_currentSceneHandle == null)
            {
                return;
            }
            
            var handle  = Addressables.UnloadSceneAsync(_currentSceneHandle.Value);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _currentSceneHandle = null;
                return;
            }
            
            throw new Exception($"#SceneManager# Failed to unload current scene: {handle.OperationException}");
        }
    }
}