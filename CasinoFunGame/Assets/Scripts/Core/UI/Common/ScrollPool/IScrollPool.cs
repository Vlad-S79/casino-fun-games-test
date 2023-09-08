namespace Core.Ui.Common.ScrollPool
{
    public interface IScrollPool
    {
        public void Init(ScrollPoolData scrollPoolData);
        public void UpdateViewport();
        public void UpdatePosition();
    }

    public enum ScrollPoolType
    {
        Vertical,
        Horizontal
    }
}