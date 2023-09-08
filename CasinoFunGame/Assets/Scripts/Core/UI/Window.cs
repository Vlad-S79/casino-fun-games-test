using UnityEngine;
using Zenject;

namespace Core.Ui
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private WindowType _windowType;
        [SerializeField] private bool _isHaveAnimation;

        protected UiComponent _uiComponent;

        [Inject]
        private void Init(UiComponent uiComponent)
        {
            _uiComponent = uiComponent;
        }
        
        public void Open()
        {
            // var siblingIndex = _uiComponent.GetSiblingIndex(_windowType);
            var siblingIndex = 0;
            transform.SetSiblingIndex(siblingIndex);

            if (!_isHaveAnimation)
            {
                gameObject.SetActive(true);
            }
            
            OnOpen();
        }
        
        public void Close()
        {
            OnClose();

            if (!_isHaveAnimation)
            {
                gameObject.SetActive(false);
            }
        }
        
        protected abstract void OnOpen();
        protected abstract void OnClose();

        public WindowType GetWindowType() => _windowType;
    }
}