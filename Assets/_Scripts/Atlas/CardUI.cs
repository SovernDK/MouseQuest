using Atlas.Utility;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private MMF_Player _pointerEnterFeel;
    [SerializeField]
    private MMF_Player _pointerExitFeel;

    [SerializeField]
    private bool _hoverSfx; 
    
    [SerializeField]
    private bool _clickSfx; 

    private int _childIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_clickSfx) 
        {
            GetComponent<AudioSource>().clip = Config.Instance.ButtonClickSfx;
            GetComponent<AudioSource>().Play();       
        }
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

            _pointerEnterFeel.PlayFeedbacks();
            _childIndex = GetComponent<RectTransform>().GetSiblingIndex(); 
            GetComponent<RectTransform>().DOBlendableLocalMoveBy(Vector3.up * 50, .5f);
            GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(GetComponent<Button>().interactable)
        {
            _pointerExitFeel.PlayFeedbacks();
        }

        GetComponent<RectTransform>().DOBlendableLocalMoveBy(Vector3.up * -50, .5f);
        GetComponent<RectTransform>().SetSiblingIndex(_childIndex);
    }
}
