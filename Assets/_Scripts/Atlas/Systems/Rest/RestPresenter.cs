using Atlas.Systems;
using Atlas.UI;
using Atlas.Views;

namespace Atlas.Presenters
{
    public class RestPresenter : IPresenter<RestSystem, RestView>
    {
        public RestSystem System { get; set; }
        public RestView View { get; set; }

        public void Initialize()
        {
            View.Initialize();
        }

        public void Enter()
        {
            View.Enter();
        }

        public void End()
        {
            View.End();
        }
    }
}