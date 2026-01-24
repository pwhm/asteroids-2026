using UnityEngine;

namespace Modules.Wrappables
{
    public interface IWrappable
    {
        public Vector3 Position { get; }
        public Vector2 Size { get; }
        public WrappableState CurrentState { get; }

        public void UpdateState(WrappableState state);
    }
}