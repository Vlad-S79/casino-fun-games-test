namespace Game.SlotsLogic
{
    public class SlotsResult
    {
        public int Coins;
        public SlotItems[] Slots;
        public ResultType ResultType;

        public SlotsResult()
        {
            Slots = new SlotItems[3];
        }
    }
}