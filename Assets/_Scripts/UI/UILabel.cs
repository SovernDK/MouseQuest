using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILabel : MonoBehaviour, IViewAnimationHandler
{
    public Ease ease;
    public TMP_Text label;

    public Tweener FadeIn(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text =>{
            text.DOFade(1, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text =>{
            text.DOFade(1, duration);
        });
        
        return GetComponent<Image>().DOFade(1, duration);
    }

    public Tweener FadeOut(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text =>{
            text.DOFade(0, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text =>{
            text.DOFade(0, duration);
        });

        return GetComponent<Image>().DOFade(0, duration);
    }

    public Tweener Fold(float duration)
    {
        GetComponent<RectTransform>().DOScaleX(1, 0);
        return GetComponent<RectTransform>().DOScaleX(0, duration).SetEase(ease);
    }

    public Tweener Unfold(float duration)
    {
        GetComponent<RectTransform>().DOScaleX(0, 0);
        return GetComponent<RectTransform>().DOScaleX(1, duration).SetEase(ease);
    }

    public void SetText(string text)
    {
        label.text = text;
    }
}
