using Core.Services;
using UnityEngine;

namespace Modules.Wrappables
{
    public interface IWrappablesService : IService
    {
        public void Register(IWrappable wrappable);
        public void Unregister(IWrappable wrappable);
    }
}