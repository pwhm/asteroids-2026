using UnityEngine;

namespace Modules.Wrappables
{
    public interface IWrappablesService
    {
        public void Register(IWrappable wrappable);
        public void Unregister(IWrappable wrappable);
    }
}