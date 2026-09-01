using Atlas.UI;
using DG.Tweening;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogueView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private TMP_Text _lines;
    [SerializeField]
    private TMP_Text _speaker;

    [SerializeField]
    private Localize _localization;

    [Inject]
    private DialoguePresenter _presenter;

    public string language = "English";

    public string ViewName => "Dialogue";

    public bool Visible => _content.gameObject.activeSelf;

    public void Initialize()
    {
        _presenter.View = this;
        LocalizationManager.CurrentLanguage = language;
    }

    public void SetText(string lines)
    {
        _lines.text = LocalizationManager.GetTranslation(lines);
    }

    public void SetSpeaker(string speaker)
    {
        _speaker.text = speaker;
    }

    public void FadeInDialogue()
    {
        _lines.GetComponent<Image>().DOFade(0, 0);
        Show();
        _lines.GetComponent<Image>().DOFade(1, 1);
    }

    public void FadeOutDialogue()
    {
        _lines.GetComponent<Image>().DOFade(0, 1).OnComplete(() => {
            Hide();
        });
    }

    public void DrawLine(/*Line line*/)
    {
        // _lines.text = LocalizationManager.GetTranslation(line.key);
        // _speaker.text = LocalizationManager.GetTranslation(line.speaker);
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }

    public void Show()
    {
        _content.gameObject.SetActive(true);
    }
}
