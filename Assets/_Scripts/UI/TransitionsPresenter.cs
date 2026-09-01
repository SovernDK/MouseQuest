using System.Collections;
using Atlas.UI;

public class TransitionsPresenter : IPresenter<TransitionsSystem, TransitionsView>
{
    public TransitionsSystem System { get; set; }
    public TransitionsView View { get; set; }

    public IEnumerator TransitionLeftToRight()
    {
        yield return View.TransitionLeftToRight();
    }

    public IEnumerator TransitionFadeIn()
    {
        yield return View.TransitionFadeIn();
    }
    public IEnumerator TransitionFadeOut()
    {
        yield return View.FadeOut();
    }

    public IEnumerator FadeOut()
    {
        yield return View.FadeOut();
    }
}
