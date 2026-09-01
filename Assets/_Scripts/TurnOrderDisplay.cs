using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Atlas.UI
{
    public class TurnOrderDisplay : MonoBehaviour
    {
        public Transform content;
        public GameObject battlerRowPrefab;

        private List<GameObject> _battlers;

        private void Awake() {
            _battlers = new List<GameObject>();
        }

        public void UpdateTurnOrder(Queue<Battler> _combatantOrder)
        {
            Queue<Battler> _orderClone = new Queue<Battler>(_combatantOrder);
            _battlers.ForEach(s => s.SetActive(false)); 

            int index = 0;
            while(_orderClone.Count > 0)
            {
                Battler combatant = _orderClone.Dequeue();
                if(index < _battlers.Count)
                {
                    // _battlers[index].GetComponentInChildren<TMP_Text>().text = $"{index+1}.{combatant.Name}";
                    _battlers[index].GetComponent<TurnOrderCell>().SetCombatantNameAndOrder(index+1, combatant.Name);
                    _battlers[index].gameObject.SetActive(true);
                }
                else
                {
                    GameObject clone = Instantiate(battlerRowPrefab, content);
                    clone.GetComponent<TurnOrderCell>().SetCombatantNameAndOrder(index+1, combatant.Name);
                    // clone.GetComponentInChildren<TMP_Text>().text = $"{index+1}.{combatant.Name}";
                    
                    _battlers.Add(clone);
                }
                index++;
            }
        }
    }
}
