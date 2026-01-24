using UnityEngine;

namespace Modules.Wrappables
{
    public struct WrappableState
    {
        // Left over from trying to be fancy and doing smooth transition, maybe will have time to do
        // it properly
        public readonly bool IsWrapping;
        public readonly bool WrapComplete;
        public readonly Vector3 WrappedPosition;

        public WrappableState(Vector3 wrappedPosition, bool isWrapping, bool wrapComplete)
        {
            WrappedPosition = wrappedPosition;
            IsWrapping = isWrapping;
            WrapComplete = wrapComplete;
        }
    }
}