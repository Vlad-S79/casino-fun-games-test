using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Ui.Common.ScrollPool
{
    public class ScrollPool<T> : IScrollPool
    {
        private List<ScrollElement> _container;
        private ScrollPoolData _scrollPoolData;

        private Action<T> _onInitAction;
        private Action<int, T> _onSetElementAction;

        private float _elementSize;
        private float _viewportSize;
        private float _position;
        private int _elementCount;
        private int _elementAmount;
        private float _containerSize;
        private float _spacing;

        private int _currentElementIndex;

        public ScrollPool(Action<T> onInitAction = null)
        {
            _onInitAction = onInitAction;
        }

        public void Init(ScrollPoolData scrollPoolData)
        {
            _container = new List<ScrollElement>();
            _scrollPoolData = scrollPoolData;

            _elementSize = _scrollPoolData.ScrollPoolType == ScrollPoolType.Horizontal
                ? _scrollPoolData.Element.rect.width
                : _scrollPoolData.Element.rect.height;

            _spacing = _scrollPoolData.Spacing;
            
            UpdateViewportValue();
        }

        public void SetCount(int count)
        {
            var axis = _scrollPoolData.ScrollPoolType == ScrollPoolType.Horizontal
                ? RectTransform.Axis.Horizontal
                : RectTransform.Axis.Vertical;

            _elementAmount = count;
            _containerSize = _elementAmount * (_elementSize + _spacing) - _spacing;
            
            _scrollPoolData.Container.SetSizeWithCurrentAnchors(axis, _containerSize);
            
            Update();
        }

        private void UpdatePositionValue()
        {
            _position = _scrollPoolData.ScrollPoolType == ScrollPoolType.Horizontal
                ? _scrollPoolData.Position.x : 1 - _scrollPoolData.Position.y;
        }

        public void UpdatePosition()
        {
            UpdatePositionValue();
            Update();
        }

        private void UpdateViewportValue()
        {
            _viewportSize = _scrollPoolData.ScrollPoolType == ScrollPoolType.Horizontal
                ? _scrollPoolData.ViewportSize.x
                : _scrollPoolData.ViewportSize.y;

            _elementCount = Mathf.FloorToInt(_viewportSize / (_elementSize + _spacing));

            while (_container.Count < _elementCount + 3)
            {
                _container.Add(InitElement());
            }
        }
        
        public void UpdateViewport()
        {
            UpdateViewportValue();
            Update();
        }

        private ScrollElement InitElement()
        {
            var reference = _scrollPoolData.Element;
            reference.gameObject.SetActive(false);
            var element = Object.Instantiate(reference, _scrollPoolData.Container, false);
            element.name = "element_" + _container.Count;
            reference.gameObject.SetActive(true);

            var tElement = element.GetComponent<T>();
            _onInitAction?.Invoke(tElement);

            return new ScrollElement
            {
                Data = tElement,
                RectTransform = element
            };
        }

        public void OnSetElementAction(Action<int, T> onSetElementAction) 
            => _onSetElementAction = onSetElementAction;

        public void Update()
        {
            var currentElement = Mathf.FloorToInt( Mathf.Lerp(0, _elementAmount - _elementCount, _position)) - 1;

            var enumerator = _container.GetEnumerator();
            var count = 0;

            while (enumerator.MoveNext())
            {
                var element = enumerator.Current;
                var index = currentElement + count;
                
                var elementGameObject = element.RectTransform.gameObject;
                if (index < 0 || index >= _elementAmount)
                {
                    elementGameObject.SetActive(false);
                    count++;
                    continue;
                }
                
                elementGameObject.SetActive(true);
                
                var positionElement = element.RectTransform.anchoredPosition;
                var position = (_elementSize + _spacing) * index;

                if (_scrollPoolData.ScrollPoolType == ScrollPoolType.Horizontal)
                    positionElement.x = position;
                else positionElement.y = -position;

                element.RectTransform.anchoredPosition = positionElement;

                _onSetElementAction?.Invoke(index, element.Data);
                count++;
            }
            enumerator.Dispose();
        }

        private class ScrollElement
        {
            public T Data;
            public RectTransform RectTransform;
        }
    }
}