using Atlas.UI;

public class DialoguePresenter : IPresenter<DialogueSystem, DialogueView>
{
    public DialogueSystem System { get; set; }
    public DialogueView View { get; set; }

    public void DrawLine(/*Line line*/)
    {
        View.DrawLine();
    }

    public void StartDialogue(/*Line line*/)
    {
        View.DrawLine();
        View.Show();
    }

    public void EndDialogue()
    {
        View.Hide();
    }
}
