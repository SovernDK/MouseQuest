using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Atlas.Utility;
using Sirenix.Utilities;
using Atlas.UI;

namespace Atlas.Core 
{
    public class ViewSystem : Singleton<ViewSystem>
    {
        protected Dictionary<string, IView> _views;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public void Initialize()
        {
            LoadAndInitializeViews();
        }

        public virtual void ShowView(string name)
        {
            if (_views.TryGetValue(name, out IView view))
            {
                view.Show();
            }
            else
            {
                Debug.LogError("Couldn't find view " + name);
            }
        }

        public virtual void HideView(string name)
        {
            if (_views.TryGetValue(name, out IView view))
            {
                view.Hide();
            }
        }

        public virtual IView GetView(string name)
        {
            if (_views.TryGetValue(name, out IView view))
            {
                return view;
            }

            return null;
        }

        public void HideAll()
        {
            _views.Values.ForEach(view => {
            view.Hide(); 
            });
        }

        public void ShowAll()
        {
            _views.Values.ForEach(view => {
            view.Show(); 
            });
        }

        public void ToggleView(string view)
        {
            _views.Values.ForEach(view => {
                if(view.Visible) 
                    view.Hide();
                else
                    view.Show();
            });
        }

        protected virtual void LoadAndInitializeViews()
        {
            _views = new Dictionary<string, IView>();
            IView[] viewInScene = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).OfType<IView>().ToArray();

            foreach (IView view in viewInScene)
            {
                view.Initialize();
                _views.Add(view.ViewName, view);
                Debug.Log("View " + view.ViewName + " Initialized!");
            }
        }
    }
}