using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui.Common.ScrollPool
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollPoolContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform _element;
        [SerializeField] private float _spacing;
        [SerializeField] private ScrollPoolType _scrollPoolType;
        
        private ScrollRect _scrollRect;
        private RectTransform _rectTransform;

        private IScrollPool _scrollPool;
        private ScrollPoolData _scrollPoolData;

        public void Init(IScrollPool scrollPool)
        {
            if (_element == null)
            {
                Debug.LogError(" *** ScrollPool Element == null");
                return;
            }
            
            _scrollPool = scrollPool;

            _scrollRect = GetComponent<ScrollRect>();
            _rectTransform = GetComponent<RectTransform>();

            _scrollPoolData = new ScrollPoolData();
            
            _scrollPoolData.Container = _scrollRect.content;
            _scrollPoolData.Element = _element;
            _scrollPoolData.ViewportSize = _rectTransform.rect.size;
            _scrollPoolData.ScrollPoolType = _scrollPoolType;
            _scrollPoolData.Position = _scrollRect.normalizedPosition;
            _scrollPoolData.Spacing = _spacing;
            
            _scrollPool.Init(_scrollPoolData);
            _scrollRect.onValueChanged.AddListener(OnChangePosition);
        }

        private void OnChangePosition(Vector2 position)
        {
            _scrollPoolData.Position = position;
            _scrollPool.UpdatePosition();
        }

        private void OnRectTransformDimensionsChange()
        {
            _scrollPoolData.ViewportSize = _rectTransform.rect.size;
            _scrollPool.UpdateViewport();
        }
    }
}