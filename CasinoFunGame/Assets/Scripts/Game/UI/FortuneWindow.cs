using System;
using System.Threading.Tasks;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.UI
{
    public class FortuneWindow : Window
    {
        [Space]
        [SerializeField] private AnimationCurve _wheelAnimationCurve;
        
        [Space]
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private Button _spinButton;
        
        private int _step = 36;
        private float _wheelSpeed = 777;
        private int _delay = 2000;
        private int _finishDelay = 2000;

        private int _offset = 4;

        private bool _isNeedRotateWheel;
        private bool _rotateFinalAngel;

        private int _finalSlot;
        private int _finalSlotIndex;

        private bool _isActive;

        private int[] _fs =
        {
            1, 5, 2, 10, 3, 1, 5, 2, 10, 3
        };

        protected override void OnOpen()
        {
            _isActive = true;
            
            _finalSlot = -1;
            _spinButton.onClick.AddListener( StartRotateWheel);
        }

        private async void StartWheelAnimationAsync()
        {
            _isNeedRotateWheel = true;
            while (_isNeedRotateWheel)
            {
                _wheelTransform.Rotate(Vector3.back, _wheelSpeed * Time.deltaTime);
                // _wheelTransform.Rotate(Vector3.back, _step);
                await Task.Yield();
                
                if (_finalSlot != -1 && 
                     _wheelTransform.eulerAngles.z >= _finalSlot - _step / 2 + _offset && 
                     _wheelTransform.eulerAngles.z <= _finalSlot + _step / 2 - _offset)
                {
                    _wheelTransform.rotation = Quaternion.Euler(0,0,_finalSlot);
                    _isNeedRotateWheel = false;
                    _finalSlot = -1;

                    FinishSpin();
                }
            }

            await Task.Delay(_finishDelay);
            Close();
        }

        private void FinishSpin()
        {
            Debug.Log(_fs[_finalSlotIndex]);
        }

        private async void StartRotateWheel()
        {
            if(!_isActive) return;
            _isActive = true;
            
            _finalSlotIndex = new Random().Next(9);
            StartWheelAnimationAsync();
            await Task.Delay(_delay);
            _finalSlot = _finalSlotIndex * _step;
        }
        
        protected override void OnClose()
        {
            _spinButton.onClick.RemoveAllListeners();
        }
    }
}