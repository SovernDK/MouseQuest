using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.Utilities;
using Atlas.Effects;
using Zenject;
using Atlas.Utility;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonAdditional : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IViewAnimationHandler
{
    [SerializeField]
    private Vector3 _scale = Vector3.one * 1.1f;
    [SerializeField]
    private bool _hoverShakeScale; 
    [SerializeField]
    private bool _clickShakeScale; 
    [SerializeField]
    private bool _hoverSfx; 
    [SerializeField]
    private bool _clickSfx; 

    [SerializeField]
    private Color _disabled;
    [SerializeField]
    private List<Image> _elements;

    [SerializeField]
    private MMF_Player _enter;
    [SerializeField]
    private MMF_Player _exit;
    [SerializeField]
    private MMF_Player _clicked;
    [SerializeField]
    private MMF_Player _deactivate;

    [Inject]
    private EffectsSystem _effects;

    public Ease ease;

    public void Deactivate()
    {
        if(_deactivate != null) _deactivate.PlayFeedbacks();
    }

    public Tweener FadeIn(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text => 
        {
            text.DOFade(1, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text => 
        {
            text.DOFade(1, duration);
        });
        
        return GetComponent<Button>().image.DOFade(1, duration);
    }

    public Tweener FadeOut(float duration)
    {
        transform.GetComponentsInChildren<TMP_Text>().ForEach(text => 
        {
            text.DOFade(0, duration);
        });

        transform.GetComponentsInChildren<Image>().ForEach(text => 
        {
            text.DOFade(0, duration);
        });

        return GetComponent<Button>().image.DOFade(0, duration);
    }

    public Tweener Unfold(float duration)
    {
        GetComponent<RectTransform>().DOScale(0, 0);
        return GetComponent<RectTransform>().DOScale(1, duration).SetEase(ease);
    }

    public Tweener Fold(float duration)
    {
        GetComponent<RectTransform>().DOScale(1, 0);
        return GetComponent<RectTransform>().DOScale(0, duration).SetEase(ease);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_clickSfx) 
        {
            GetComponent<AudioSource>().clip = Config.Instance.ButtonClickSfx;
            GetComponent<AudioSource>().Play();       
        }

        if(_clickShakeScale) 
            GetComponent<RectTransform>().DOShakeScale(.1f, 1, 3, 90, true, ShakeRandomnessMode.Harmonic);
            
        if(_clicked != null) 
            _clicked.PlayFeedbacks();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GetComponent<Button>().interactable)
        {
            if(_hoverSfx) 
            {
                GetComponent<AudioSource>().clip = Config.Instance.ButtonHoverSfx;
                GetComponent<AudioSource>().Play();       
            }

            if(_hoverShakeScale)
                GetComponent<RectTransform>().DOScale(_scale, .2f);
        }

        if(_enter != null) 
            _enter.PlayFeedbacks();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(GetComponent<Button>().interactable)
        {
            if(_hoverShakeScale) 
                GetComponent<RectTransform>().DOScale(Vector3.one, .2f);
        }
        
        if(_exit != null) 
            _exit.PlayFeedbacks();
    }
}
