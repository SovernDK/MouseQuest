using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [TitleGroup("Shake")] [SerializeField]
    private float _duration;
    [TitleGroup("Shake")] [SerializeField]
    private float _strength;
    [TitleGroup("Shake")] [SerializeField]
    private int _vibrato;

    [Button("Shake")]
    public void Shake()
    {
        // GetComponent<Camera>().DOShakePosition(_duration, _strength, _vibrato);
        GetComponent<MMShaker>().Play();
    }
}
