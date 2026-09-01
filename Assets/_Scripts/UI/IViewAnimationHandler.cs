using DG.Tweening;

public interface IViewAnimationHandler
{
    public Tweener FadeIn(float duration);
    public Tweener FadeOut(float duration);
    public Tweener Unfold(float duration);
    public Tweener Fold(float duration);
}