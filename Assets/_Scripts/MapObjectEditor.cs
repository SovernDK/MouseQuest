using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.MapEditor 
{
    public class MapObjectEditor : MonoBehaviour
    {
        [SerializeField] [ValueDropdown("@Terms()")]
        private string type = "Battle";   

        private string[] Terms()
        {
            return new string[1] { "Battle" };
        }
    }
}