using UnityEngine;

namespace Modules.Wrappables
{
    public interface IWrappable
    {
        public Vector3 Position { get; }

        public void UpdateState(WrappableState state);
    }
}