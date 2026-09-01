using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderAdditional : MonoBehaviour, IViewAnimationHandler
{
    public Tweener FadeIn(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text =>{
            text.DOFade(1, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text =>{
            text.DOFade(1, duration);
        });

        return GetComponent<Slider>().image.DOFade(1, duration);
    }

    public Tweener FadeOut(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text =>{
            text.DOFade(0, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text =>{
            text.DOFade(0, duration);
        });
       
        return GetComponent<Slider>().image.DOFade(0, duration);
    }

    public Tweener Unfold(float duration)
    {
        GetComponent<Slider>().DOValue(0, 0);
        return GetComponent<Slider>().DOValue(1, duration);
    }

    public Tweener Fold(float duration)
    {
        GetComponent<Slider>().DOValue(1, 0);
        return GetComponent<Slider>().DOValue(0, duration);
    }
}
