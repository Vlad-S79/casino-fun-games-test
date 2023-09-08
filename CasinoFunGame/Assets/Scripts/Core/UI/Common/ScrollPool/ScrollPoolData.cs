using UnityEngine;

namespace Core.Ui.Common.ScrollPool
{
    public class ScrollPoolData
    {
        public RectTransform Element;
        public RectTransform Container;
        public Vector2 ViewportSize;
        public Vector2 Position;
        public float Spacing;
        public ScrollPoolType ScrollPoolType;
    }
}