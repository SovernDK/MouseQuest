namespace Atlas.UI 
{
    public interface IView
    {
        public string ViewName { get; }
        public bool Visible { get; }
        public void Initialize();
        public void Show();
        public void Hide();
    }
}