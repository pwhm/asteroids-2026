using System.Threading.Tasks;
using Core.Services;
using UnityEngine;

namespace Modules.Assets
{
    public interface IAssetService : IService
    {
        public Task<T> LoadPrefab<T>(string key, string tag) where T : MonoBehaviour;
        public void ReleaseAssets(string tag);
    }
}