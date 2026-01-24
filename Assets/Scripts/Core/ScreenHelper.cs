
using UnityEngine;

namespace Core
{
    public readonly struct ScreenBounds
    {
        public readonly float Left;
        public readonly float Top;
        public readonly float Right;
        public readonly float Bottom;

        public readonly Vector2 Size;

        public ScreenBounds(float left, float right, float top, float bottom, Vector2 size)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Size = size;
        }

        public bool IsWithinBounds(Vector3 position)
        {
            if (position.x < Left || position.x > Right)
            {
                return false;
            }
            
            if (position.y > Top || position.y < Bottom)
            {
                return false;
            }
            
            return true;
        }
    }
    
    public static class ScreenHelper
    {
        public static ScreenBounds GetScreenBounds(Camera camera)
        {
            var height = camera.orthographicSize * 2;
            var width = height * camera.aspect;

            return new ScreenBounds(-width / 2, width / 2, height / 2, -height / 2, new Vector2(width, height));
        }
    }
}