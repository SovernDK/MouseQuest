using Atlas.Core;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenView : MonoBehaviour
{
    public Button newGame;

    public Transform content;
    public GameObject newGameButton;

    public void Initialize() 
    {
        GameObject newGameButtonClone = Instantiate(newGameButton, content);
        GameStateSystem gm = FindAnyObjectByType<GameStateSystem>();
        newGameButtonClone.GetComponent<Button>().onClick.AddListener(() => { content.gameObject.SetActive(false); gm.SetState(EGameState.CharacterCreator); });
    }
}
