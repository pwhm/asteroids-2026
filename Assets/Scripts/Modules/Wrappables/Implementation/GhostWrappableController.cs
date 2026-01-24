using Core.Services;
using UnityEngine;

namespace Modules.Wrappables.Implementation
{
    internal sealed class GhostWrappableController : MonoBehaviour, IWrappable
    {
        public Vector3 Position => transform.position;
        public Vector2 Size => _collider.bounds.size;
        public WrappableState CurrentState { get; private set; }

        [field: SerializeField] private Collider2D _collider;

        private void OnEnable()
        {
            Services.GetService<IWrappablesService>().Register(this);
        }

        private void OnDisable()
        {
            Services.GetService<IWrappablesService>().Unregister(this);
        }
        
        public void UpdateState(WrappableState state)
        {
            CurrentState = state;
            
            // Probably better to do this with enum and a switch?
            if (state.WrapComplete)
            {
                transform.position = state.WrappedPosition;
            }
        }
    }
}