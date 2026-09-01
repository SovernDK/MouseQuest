namespace Atlas.UI 
{
    public interface IPresenter<S, V>
    {
        public S System { get; set; }   
        public V View { get; set; }   
    }
}