using UnityEngine;

namespace Atlas.Effects
{
    public interface IEffect
    {
        public string EffectName { get; }
        public float EffectTime { get; }
        public Transform Follow { get; set;  }
        public bool IsFollowing  { get; set; }

        public void Play();
    }

    public interface ICanvasEffect 
    {
        public string EffectName { get; }
        public Transform Follow { get;set;  }
        public bool IsFollowing  { get; set; }

        public void Play();
    }
}
