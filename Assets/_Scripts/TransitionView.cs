using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas.UI
{
    public class TransitionView : MonoBehaviour
    {
        public Image fade;

        public Tweener FadeIn(float time = .5f)
        {
            return fade.DOFade(1, time);
        }

        public Tweener FadeOut(float time = 1f)
        {
            return fade.DOFade(0, time);
        }
    }
}
