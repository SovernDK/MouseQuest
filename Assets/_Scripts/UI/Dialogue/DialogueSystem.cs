using DB;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DialogueSystem : MonoBehaviour
{
    // private Dialogue _currentDialogue;
    private int _index;

    [Inject]
    public DialoguePresenter Presenter { get; set; }

    public UnityEvent DialogueStarted { get; set; }
    public UnityEvent DialogueEnded { get; set; }

    public void Initialize() 
    {
        Presenter.System = this;    
        DialogueStarted = new UnityEvent();
        DialogueEnded = new UnityEvent();
    }

    public void StartDialogue(int dialogueId)
    {
        DialogueStarted.Invoke();
        // _currentDialogue = DepotSystem.Dialogues[dialogueId];
        _index = 0;

        // Presenter.StartDialogue(_currentDialogue.lines[_index]);
    }

    public void NextLine()
    {
        // if(_currentDialogue.lines.Count > _index + 1)
        // {
        //     _index++;
        //     Presenter.DrawLine(_currentDialogue.lines[_index]);
        // }
        // else
        // {
        //     DialogueEnded.Invoke();
        //     Presenter.EndDialogue();
        // }
    }
}
