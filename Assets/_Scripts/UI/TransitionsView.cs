using System.Collections;
using Atlas.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TransitionsView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _curtain;
    
    [SerializeField]
    private Transform _fade;

    private RectTransform _curtainTransform;
    private Image _curtainImage;
    
    private RectTransform _fadeTransform;
    private Image _fadeImage;
    
    public string ViewName => "Transition";
    public bool Visible => false;

    [Inject]
    public TransitionsPresenter Presenter { get; set; }

    public void Initialize()
    {
        Presenter.View = this;
        
        _curtainTransform = _curtain.gameObject.GetComponent<RectTransform>();
        _curtainImage = _curtain.gameObject.GetComponent<Image>();

        _fadeTransform = _fade.gameObject.GetComponent<RectTransform>();
        _fadeImage = _fade.gameObject.GetComponent<Image>();
    }

    public void Hide()
    {
       
    }

    public void Show()
    {
        
    }

    public IEnumerator TransitionLeftToRight()
    {
        _curtainTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        // // Move the panel from off-screen left to cover the entire screen
        _curtainTransform.anchoredPosition = new Vector2(-Screen.width, _curtainTransform.anchoredPosition.y);

        yield return _curtainTransform.DOAnchorPosX(-Screen.width, 0f).WaitForCompletion();
        _curtainImage.color = Color.black;
        _curtain.gameObject.SetActive(true);
        yield return _curtainTransform.DOAnchorPosX(0, 1f).WaitForCompletion();
    }

    public IEnumerator TransitionFadeIn()
    {
        _fadeImage.DOFade(0,0);
        _fade.gameObject.SetActive(true);
        yield return _fadeImage.DOFade(1,1).WaitForCompletion();
    }

    public IEnumerator FadeOut()
    {
        yield return _fadeImage.DOFade(0,1).WaitForCompletion();
        _fade.gameObject.SetActive(false);
    }
}
