using System.Collections;
using Atlas.Core;
using UnityEngine;
using Zenject;

public class TransitionsSystem : MonoBehaviour, ISystem
{
    [Inject]
    public TransitionsPresenter Presenter { get; set; }
    
    private void Awake() 
    {
        Presenter.System = this;
    }

    public void Initialize()
    {

    }

    public IEnumerator TransitionLeftToRight()
    {
        yield return Presenter.TransitionLeftToRight();
    }

    public IEnumerator FadeIn()
    {
        yield return Presenter.TransitionFadeIn();
    }

    public IEnumerator FadeOut()
    {
        yield return Presenter.FadeOut();
    }
}
