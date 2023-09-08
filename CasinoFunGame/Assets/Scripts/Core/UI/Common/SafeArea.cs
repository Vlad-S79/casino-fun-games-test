using System;
using UnityEngine;

namespace Core.Ui.Common
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private Vector4 _safeArea;

        private Action _onChangeRect;
        private bool _isInit;
        
        private void Start()
        {
            _safeArea = new Vector4();
            
            UpdateRectSize();
            
            _isInit = true;
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateRectSize();
        }

        private void UpdateRectSize()
        {
            var safeArea = Screen.safeArea;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            _safeArea.x = safeArea.x / screenWidth;
            _safeArea.y = (safeArea.x + safeArea.width) / screenWidth;
            _safeArea.z = safeArea.y / screenHeight;
            _safeArea.w = (safeArea.y + safeArea.height) / screenHeight;

            _onChangeRect?.Invoke();
        }

        public void AddListener(Action onChangeRect)
        {
            if(_isInit) onChangeRect?.Invoke();
            _onChangeRect += onChangeRect;
        }

        public void ChangeTop(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, _safeArea.w);
        }

        public void ChangeBottom(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, _safeArea.z);
            rectTransform.anchorMax = new Vector2(1, 1);
        }

        public void ChangeLeft(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(_safeArea.x, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }

        public void ChangeRight(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(_safeArea.y, 1);
        }

        public void ChangeHorizontal(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(_safeArea.x, 0);
            rectTransform.anchorMax = new Vector2(_safeArea.y, 1);
        }

        public void ChangeVertical(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, _safeArea.z);
            rectTransform.anchorMax = new Vector2(1, _safeArea.w);
        }

        public void Change(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(_safeArea.x, _safeArea.z);
            rectTransform.anchorMax = new Vector2(_safeArea.y, _safeArea.w);
        }
    }
}