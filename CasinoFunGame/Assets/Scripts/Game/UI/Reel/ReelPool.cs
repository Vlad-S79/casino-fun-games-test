using System;
using System.Threading.Tasks;
using Game.SlotsLogic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.UI.Reel
{
    [RequireComponent(typeof(RectTransform))]
    public class ReelPool : MonoBehaviour
    {
        [SerializeField] private Image _itemImageRef;
        
        private float _spacing = 20f;
        private float _spinSpeed = 777;
        
        private RectTransform _rectTransform;
        private ReelPoolData _reelPoolData;

        private Vector2 _position;

        private int _currentIndex;
        private bool _isNeedAnimation;

        private float _refSize;

        private Image[] _images;

        private int _stopItem = -1;
        private Action _onComplete;

        public void Init(ReelPoolData reelPoolData)
        {
            _currentIndex = new Random().Next(6);
            _reelPoolData = reelPoolData;
            _rectTransform = GetComponent<RectTransform>();

            _position = _rectTransform.localPosition;
            
            InitReelPool();
        }

        private void InitReelPool()
        {
            var size = _rectTransform.rect.height;
            _refSize = _itemImageRef.rectTransform.rect.height;
            
            var amount = (size + _spacing) / (_refSize + _spacing);
            amount = Mathf.Round(amount) + 1;

            _images = new Image[(int)amount + 1];

            for (var i = -1; i < amount; i++)
            {
                var image = Instantiate(_itemImageRef, transform);
                image.transform.localPosition = new Vector3(0, i * (_spacing + _refSize), 0);
                _images[i + 1] = image;
            }

            UpdateImage();
        }

        private void UpdateImage()
        {
            var index = -1;
            foreach (var image in _images)
            {
                image.sprite = GetByIndex(_currentIndex + index);
                index++;
            }
        }

        private Sprite GetByIndex(int index)
        {
            if (index < 0) index = 7 + index;
            if (index >= 7) index -= 7;

            return _reelPoolData.SlotItemsSprite[index];
        }

        public async void RunAnimationAsync()
        {
            _isNeedAnimation = true;
            while (_isNeedAnimation)
            {
                _position.y -= Time.deltaTime * _spinSpeed;
                _rectTransform.localPosition = _position;
                
                await Task.Yield();
                CheckPosition();
            }
        }

        private void CheckPosition()
        {
            if (_rectTransform.anchoredPosition.y < -(_refSize + _spacing))
            {
                _position.y = 0;
 
                _currentIndex++;
                if (_currentIndex > 6)
                {
                    _currentIndex = 0;
                }
      
                UpdateImage();
                
                if (_currentIndex == _stopItem)
                {
                    _rectTransform.localPosition = _position;

                    _isNeedAnimation = false;
                    _onComplete?.Invoke();
                    _onComplete = null;
                    _stopItem = -1;
                }
            }
        }

        private void OnDisable()
        {
            _isNeedAnimation = false;
        }

        public void StopOnSlotItem(SlotItems slotItems, Action onComplete)
        {
            _stopItem = (int) slotItems;
            _onComplete = onComplete;
        }

        public async void StopOnSlotItemAsync(SlotItems slotItems, int delayMilliseconds, Action onComplete)
        {
            await Task.Delay(delayMilliseconds);
            StopOnSlotItem(slotItems, onComplete);
        }
    }
}