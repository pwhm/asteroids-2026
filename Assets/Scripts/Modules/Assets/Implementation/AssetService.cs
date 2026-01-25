using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var assets = _loadedAssets[tag];

            foreach (var asset in assets)
            {
                Addressables.Release(asset.Value);
            }
            
            _loadedAssets[tag].Clear();
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