using System;

namespace Game.SlotsLogic
{
    public class SlotsLogicComponent
    {
        private Random _random;
        private SlotsResult _slotsResult;

        private int _wightSum;
        private byte[] _weights =
        {
            10, // Chery
            10, // Lemon
            10, // Grape
            10, // Watermelon
            5, // Dollar
            3, // Bar
            2 // Seven
        };

        public SlotsLogicComponent()
        {
            _random = new Random();
            _slotsResult = new SlotsResult();

            foreach (var weight in _weights)
            {
                _wightSum += weight;
            }
        }

        public SlotsResult Spin(int bet)
        {
            GenerateSlots();
            _slotsResult.Coins = GetCoinsResult(bet);
            
            return _slotsResult;
        }

        private void GenerateSlots()
        {
            for (var i = 0; i < 3; i++)
            {
                _slotsResult.Slots[i] = RandomSlotItem();
            }
        }

        private SlotItems RandomSlotItem()
        {
            var randomValue = _random.Next(_wightSum);

            int cumulativeWeight = 0;
            for (int i = 0; i < _weights.Length; i++)
            {
                cumulativeWeight += _weights[i];
                if (randomValue < cumulativeWeight)
                {
                    return (SlotItems) i;
                }
            }

            return SlotItems.Chery;
        }

        private int GetCoinsResult(int bet)
        {
            if (_slotsResult.Slots[0] == _slotsResult.Slots[1] &&
                _slotsResult.Slots[1] == _slotsResult.Slots[2])
            {
                _slotsResult.ResultType = ResultType.ThreePair;

                switch (_slotsResult.Slots[0])
                {
                    case SlotItems.Chery: return 10 * bet;
                    case SlotItems.Lemon: return 20 * bet;
                    case SlotItems.Grape: return 30 * bet;
                    case SlotItems.Watermelon: return 40 * bet;
                    case SlotItems.Dollar: return 50 * bet;
                    case SlotItems.Bar:
                    {
                        _slotsResult.ResultType = ResultType.BarTreePair;
                        return 100 * bet;
                    }
                    case SlotItems.Seven:
                    {
                        _slotsResult.ResultType = ResultType.SevenTreePair;
                        return 200 * bet;
                    }
                }
            }

            if (_slotsResult.Slots[0] == _slotsResult.Slots[1] ||
                _slotsResult.Slots[0] == _slotsResult.Slots[2] ||
                _slotsResult.Slots[1] == _slotsResult.Slots[2])
            {
                _slotsResult.ResultType = ResultType.TwoPair;
                return 5 * bet;
            }

            _slotsResult.ResultType = ResultType.Fail;
            return 0;
        }
    }
}