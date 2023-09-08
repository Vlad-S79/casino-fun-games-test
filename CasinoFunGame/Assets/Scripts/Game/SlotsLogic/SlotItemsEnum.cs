namespace Game.SlotsLogic
{
    public enum SlotItems : byte
    {
        Chery = 0,
        Lemon = 1,
        Grape = 2,
        Watermelon = 3,
        Dollar = 4,
        Bar = 5,
        Seven = 6
    }

    //For Animations
    public enum ResultType : byte
    {
        Fail,
        TwoPair,
        ThreePair,
        SevenTreePair,
        BarTreePair
    }
}