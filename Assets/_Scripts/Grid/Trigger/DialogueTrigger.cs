using UnityEngine;
using Zenject;
using System;
using PixelCrushers.DialogueSystem;

public class DialogueTrigger : OverworldTrigger
{
    [SerializeField]
    [ES3Serializable]
    private int _dialogueId;
    
    [Inject]
    private DialogueSystem _dialogueSystem;

    #region IPoolable
    public string Name => gameObject.name;
    public GameObject Prefab => gameObject;
    public int AmountToPool => 5;
    public bool ShouldExpand => true;
    public bool LazyPool => false;
    #endregion

    [Serializable]
    private class DialogueData
    {
        public int dialogueId;
    }

    public override void Trigger()
    {
        base.Trigger();
        GetComponent<DialogueSystemTrigger>().Fire(transform);
    }

    // #region ISerializableObject
    // public override void Save(ES3Settings settings, string path = "")
    // {
    //     DialogueData data = new DialogueData()
    //     {
    //         dialogueId = _dialogueId
    //     };

    //     ES3.Save($"DialogueTrigger_{SerializationKey}_Data", data, path);
    //     ES3.Save($"DialogueTrigger_{SerializationKey}_Trigger", GetComponent<DialogueSystemTrigger>(), path);
    //     ES3.Save($"DialogueTrigger_{SerializationKey}_Transform", transform, path);
    // }

    // public override void Load(ES3Settings settings, string path = "")
    // {
    //     Transform loadedTransform = ES3.Load<Transform>($"DialogueTrigger_{SerializationKey}_Transform", path);
    //     transform.position = loadedTransform.position;
    //     transform.rotation = loadedTransform.rotation;

    //     DialogueData data = ES3.Load<DialogueData>($"DialogueTrigger_{SerializationKey}_Data", path);
    //     _dialogueId = data.dialogueId;

    //     ES3.LoadInto($"DialogueTrigger_{SerializationKey}_Trigger", path, GetComponent<DialogueSystemTrigger>());
    // }
    // #endregion

    public void OnObjectSpawned()
    {
        
    }
}
