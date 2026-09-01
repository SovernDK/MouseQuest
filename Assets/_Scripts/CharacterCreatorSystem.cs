using Atlas.Core;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Atlas.Systems
{
    public class CharacterCreatorSystem : MonoBehaviour
    {
        private void Start() 
        {
            MMF_Player[] _players = FindObjectsByType<MMF_Player>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            foreach(MMF_Player player in _players)
            {
                player.PlayFeedbacks();
            }
        }

        public void ChooseCharacter(int id)
        {
            ES3.Save("character", id);
            GameStateSystem gm = FindAnyObjectByType<GameStateSystem>();
            gm.SetState(EGameState.Loading);
        }
    }
}
