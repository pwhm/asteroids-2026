using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;

namespace Modules.Wrappables.Implementation
{
    internal sealed class WrappablesService : MonoBehaviour, IWrappablesService
    {
        private ScreenBounds _bounds;
        private List<IWrappable> _wrappables = new();

        private void Update()
        {
            foreach (var wrappable in _wrappables)
            {
                ProcessWrappable(wrappable);
            }
        }

        public Task InitializeAsync()
        {
            // Camera.main for speed, some centralized way of holding reference to camera(s) would be better
            _bounds = ScreenHelper.GetScreenBounds(Camera.main);

            return Task.CompletedTask;
        }

        public void Register(IWrappable wrappable)
        {
            _wrappables.Add(wrappable);
        }

        public void Unregister(IWrappable wrappable)
        {
            _wrappables.Remove(wrappable);
        }

        private void ProcessWrappable(IWrappable wrappable)
        {
            var touchingTopBottom = IsTouchingVerticalEdge(wrappable);
            var touchingLeftRight = IsTouchingHorizontalEdge(wrappable);
            
            if (!touchingTopBottom && !touchingLeftRight)
            {
                return;
            }
            
            var state = CalculateCurrentWrappableState(wrappable, touchingTopBottom, touchingLeftRight);
            wrappable.UpdateState(state);
        }

        private bool IsTouchingHorizontalEdge(IWrappable wrappable)
        {
            var position = wrappable.Position;

            var size = wrappable.Size;
            var halfWidth = size.x * 0.5f;

            return position.x < _bounds.Left + halfWidth || position.x > _bounds.Right - halfWidth;
        }
        
        private bool IsTouchingVerticalEdge(IWrappable wrappable)
        {
            var position = wrappable.Position;

            var size = wrappable.Size;
            var halfHeight = size.y * 0.5f;

            return position.y < _bounds.Bottom + halfHeight || position.y > _bounds.Top - halfHeight;
        }

        private WrappableState CalculateCurrentWrappableState(
            IWrappable wrappable, 
            bool isTransitioningVertical, 
            bool isTransitioningHorizontal)
        {
            var position = wrappable.Position;

            var size = wrappable.Size;
            var halfWidth = size.x * 0.5f;
            var halfHeight = size.y * 0.5f;

            var ghostPosition = position;

            var completedVerticalTransition = false;
            if (isTransitioningVertical)
            {
                if (position.y - halfHeight > _bounds.Top)
                {
                    // RightSide
                    completedVerticalTransition = true;
                    ghostPosition.y -= _bounds.Size.y;
                }

                if (position.y + halfHeight < _bounds.Bottom)
                {
                    completedVerticalTransition = true;
                    ghostPosition.y += _bounds.Size.y;
                }
            }
            var completedHorizontalTransition = false;
            if (isTransitioningHorizontal)
            {
                if (position.x - halfHeight > _bounds.Right)
                {
                    // RightSide
                    completedHorizontalTransition = true;
                    ghostPosition.x -= _bounds.Size.x;
                }

                if (position.x + halfHeight < _bounds.Left)
                {
                    // LeftSide
                    completedHorizontalTransition = true;
                    ghostPosition.x += _bounds.Size.x;
                }
            }

            return new WrappableState(
                ghostPosition,
                isTransitioningHorizontal || isTransitioningVertical,
                completedHorizontalTransition || completedVerticalTransition);
        }
    }
}