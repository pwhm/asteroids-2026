using UnityEngine;

namespace Modules.Wrappables
{
    public struct WrappableState
    {
        public readonly bool IsWrapping;
        public readonly bool ShouldTransition;
        public readonly Vector3 Position;

        public WrappableState(Vector3 position, bool isWrapping, bool shouldTransition)
        {
            Position = position;
            IsWrapping = isWrapping;
            ShouldTransition = shouldTransition;
        }
    }
}