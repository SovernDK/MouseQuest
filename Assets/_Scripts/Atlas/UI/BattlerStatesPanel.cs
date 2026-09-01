using System.Collections.Generic;
using Atlas.DB;
using CharacterSheet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas.UI
{
    public class BattlerStatesPanel : MonoBehaviour
    {
        public GameObject stateIconPrefab;
        public Transform parent;

        private List<GameObject> _states;

        private void Awake() 
        {
            _states = new List<GameObject>();
        }

        public void UpdateStates(List<State> states)
        {
            _states.ForEach(s => s.SetActive(false)); 

            for(int i = 0; i < states.Count; i++)
            {
                BattlerState bState = Database.Instance.GetBattlerState(states[i].Id);
                if(i < _states.Count)
                {
                    _states[i].GetComponent<BattlerStateCell>().SetState(bState);
                    // _states[i].GetComponentInChildren<TMP_Text>().text = $"{states[i].TurnsLeft}";
                    // _states[i].GetComponentInChildren<Image>().sprite = bState.icon;
                    _states[i].gameObject.SetActive(true);
                }
                else
                {
                    GameObject clone = Instantiate(stateIconPrefab, parent);
                    clone.GetComponent<BattlerStateCell>().SetState(bState);

                    // clone.GetComponentInChildren<TMP_Text>().text = $"{states[i].TurnsLeft}";
                    // clone.GetComponentInChildren<Image>().sprite = bState.icon;
                    
                    _states.Add(clone);
                }
            }
        }
    }
}
