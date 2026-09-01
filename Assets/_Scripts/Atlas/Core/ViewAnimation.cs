using DG.Tweening;
using UnityEngine.UI;

public class ViewAnimation 
{
    public void FadeInOut(Graphic graphic, float inDuration, float outDuration)
    {
        graphic.DOFade(0, inDuration);
        graphic.DOFade(1, outDuration);
    }

    public Tweener FadeIn(Graphic graphic, float inDuration)
    {
        return graphic.DOFade(1, inDuration);
    }

    public Tweener FadeOut(Graphic graphic, float outDuration)
    {
        return graphic.DOFade(0, outDuration);
    }
}