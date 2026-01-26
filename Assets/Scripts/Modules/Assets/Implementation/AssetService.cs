using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modules.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Modules.Assets.Implementation
{
    internal sealed partial class AssetService : IAssetService
    {
        private Dictionary<string, Dictionary<string, AsyncOperationHandle>> _loadedAssets = new();

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
        
        public async Task<T> LoadPrefab<T>(string key, string tag) where T : MonoBehaviour
        {
            if (_loadedAssets.ContainsKey(tag) && _loadedAssets[tag].ContainsKey(key))
            {
                return (T)_loadedAssets[tag][key].Result;
            }
            
            var handle = Addressables.LoadAssetAsync<GameObject>(key);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception(handle.OperationException.Message);
            }
            
            TrackAsset(tag, key, handle);
            
            return handle.Result.GetComponent<T>();
        }

        public void ReleaseAssets(string tag)
        {
            if (!_loadedAssets.ContainsKey(tag))
            {
                return;
            }
            
            var assets = _loadedAssets[tag];

            foreach (var asset in assets)
            {
                Addressables.Release(asset.Value);
            }
            
            _loadedAssets[tag].Clear();
        }

        public async Task LoadScene(string name, string tag)
        {
            var key = string.Format(Constants.Addressables.SCENE_KEY_FORMAT, name);
            var handle = Addressables.LoadSceneAsync(key);
            
            await handle.Task;
            
            _loadedAssets[tag].Add(name, handle);
        }

        private void TrackAsset(string tag, string key, AsyncOperationHandle handle)
        {
            if (!_loadedAssets.ContainsKey(tag))
            {
                _loadedAssets.Add(tag, new Dictionary<string, AsyncOperationHandle>());
            }
            
            _loadedAssets[tag][key] =  handle;
        }
    }
}