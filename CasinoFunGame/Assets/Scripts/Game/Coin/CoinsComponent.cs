using System;
using UnityEngine;

namespace Game.Coin
{
    // I can encrypt memory and saves
    // I think this game doesn't need anything other than PlayerPrefs
    
    public class CoinsComponent
    {
        public event Action OnCoinsChange;
        
        private string _coinsKey = "coins";
        private int _defaultValue = 30000;
        
        public int Coins { get; private set; }

        public CoinsComponent()
        {
            Coins = PlayerPrefs.GetInt(_coinsKey, _defaultValue);
        }

        public bool GetCoins(int value)
        {
            var result = Coins - value;
            
            if (result >= 0)
            {
                SetCoins(result);
                return true;
            }

            return false;
        }

        public void AddCoins(int value)
        {
            var result = Coins + value;
            SetCoins(result);
        }

        private void SetCoins(int value)
        {
            var oldValue = Coins;
            Coins = value;
            
            if (oldValue != value)
            {
                OnCoinsChange?.Invoke();
            }
            
            PlayerPrefs.SetInt(_coinsKey, value);
            PlayerPrefs.Save();
        }
    }
}