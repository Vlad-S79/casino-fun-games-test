using System;
using System.Threading.Tasks;
using Core.Ui;
using Game.Coin;
using Game.Data;
using Game.SlotsLogic;
using Game.UI.Reel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class GameWindow : Window
    {
        [Space]
        [SerializeField] private ReelPool[] _reelPools;
        [SerializeField] private Button _spinButton;

        [SerializeField] private Button _k10Button;
        [SerializeField] private Button _k20Button;
        [SerializeField] private Button _k30Button;

        [SerializeField] private Button _minusButton;
        [SerializeField] private Button _addButton;

        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _autoButton;

        [SerializeField] private TextMeshProUGUI _betText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _rubyText;
        
        [SerializeField] private Transform _backgroundEffectTransform;
        
        private int _delayTimeMilliseconds = 1000;
        private int _spinTimeMilliseconds = 1000;

        private int _defaultBetValue = 100;
        private int _currentBetValue;
        
        private int _stepAddMinusBetValue = 100;

        private bool _isSpin;
        
        private ReelPoolData _reelPoolData;

        private SlotsLogicComponent _slotsLogicComponent;
        private CoinsComponent _coinsComponent;

        private int _openFortuneCounter;

        [Inject]
        private void Init(GameScriptableObject gameScriptableObject, SlotsLogicComponent slotsLogicComponent,
            CoinsComponent coinsComponent)
        {
            _coinsComponent = coinsComponent;
            _slotsLogicComponent = slotsLogicComponent;
            _reelPoolData = new ReelPoolData();

            _reelPoolData.SlotItems = gameScriptableObject.SlotItemsArray;
            _reelPoolData.SlotItemsSprite = gameScriptableObject.SlotItemsSpriteArray;

            foreach (var reelPool in _reelPools)
            {
                reelPool.Init(_reelPoolData);
            }

            Init();
            StartBackgroundAnimationAsync();
        }

        private void Init()
        {
            _coinsText.text = _coinsComponent.Coins.ToString();
            
            _currentBetValue = _defaultBetValue;
            _betText.text = _currentBetValue.ToString();

            _coinsComponent.OnCoinsChange += OnCoinsChanged;
            OnCoinsChanged();
        }

        private void OnCoinsChanged()
        {
            _coinsText.text = _coinsComponent.Coins.ToString();
        }
        
        protected override void OnOpen()
        {
            AddButtonsListeners();
        }

        private void StartSpin()
        {
            if(_isSpin) return;
            _isSpin = true;
            
            _coinsComponent.GetCoins(_currentBetValue);
            var result = _slotsLogicComponent.Spin(_currentBetValue);

            foreach (var reelPool in _reelPools)
            {
                reelPool.RunAnimationAsync();
            }
            
            _reelPools[0].StopOnSlotItemAsync(result.Slots[0],_spinTimeMilliseconds, ()=>
                _reelPools[1].StopOnSlotItemAsync(result.Slots[1],_delayTimeMilliseconds, ()=>
                    _reelPools[2].StopOnSlotItemAsync(result.Slots[2],_delayTimeMilliseconds, () =>
                        {
                            OnCompleteSpin(result);
                            _isSpin = false;
                        })));
        }

        private void OnCompleteSpin(SlotsResult slotsResult)
        {
            if (slotsResult.ResultType != ResultType.Fail)
            {
                _coinsComponent.AddCoins(slotsResult.Coins);
            }
            
            UpdateBetValueAmount();
            ForTestFortune();
        }

        private void ForTestFortune()
        {
            if (_openFortuneCounter == 0)
            {
                var window = _uiComponent.GetWindow<FortuneWindow>();
                window.Open();
                _openFortuneCounter = 30;
            }

            _openFortuneCounter--;
        }

        private async void StartBackgroundAnimationAsync()
        {
            while (_backgroundEffectTransform)
            {
                _backgroundEffectTransform.Rotate(Vector3.forward, Time.deltaTime);
                await Task.Yield();
            }
        }

        private void AddButtonsListeners()
        {
            _spinButton.onClick.AddListener(StartSpin);
            
            _addButton.onClick.AddListener(AddBetValue);
            _minusButton.onClick.AddListener(MinusBetValue);
            
            _resetButton.onClick.AddListener(()=>
                SetBetValue(Math.Min(_coinsComponent.Coins, _defaultBetValue)));
            
            _k10Button.onClick.AddListener(() => SetBetValue(10000));
            _k20Button.onClick.AddListener(() => SetBetValue(20000));
            _k30Button.onClick.AddListener(() => SetBetValue(30000));
        }

        private void SetBetValue(int value)
        {
            if(value <= 0 || value > _coinsComponent.Coins) return;
            _currentBetValue = value;
            UpdateBetValueText();
        }
        
        private void AddBetValue()
        {
            var result = _currentBetValue + _stepAddMinusBetValue;
            SetBetValue(result);
        }

        private void MinusBetValue()
        {
            var result = _currentBetValue - _stepAddMinusBetValue;
            SetBetValue(result);
        }

        private void UpdateBetValueText()
        {
            _betText.text = _currentBetValue.ToString();
        }

        private void UpdateBetValueAmount()
        {
            if (_currentBetValue > _coinsComponent.Coins)
            {
                SetBetValue(_coinsComponent.Coins);
            }
        }

        protected override void OnClose()
        {
            
        }
    }
}