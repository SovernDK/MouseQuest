using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Atlas.Core 
{
    public class ResourcesSystem
    {
        Dictionary<string, AudioClip> _clips;
        // Dictionary<string, Sprite> _miniatures;
        List<Sprite> _miniatures;
        Dictionary<string, string> _locations;
        Dictionary<string, Sprite> _spellIcons;
        Dictionary<string, Sprite> _battlers;
        Dictionary<string, VolumeProfile> _profiles;

        public Dictionary<string, AudioClip> Clips { get => _clips; set => _clips = value; }
        public Dictionary<string, string> Locations { get => _locations; set => _locations = value; }
        // public Dictionary<string, Sprite> Miniatures { get => _miniatures; set => _miniatures = value; }
        public List<Sprite> Miniatures { get => _miniatures; set => _miniatures = value; }
        public Dictionary<string, Sprite> SpellIcons { get => _spellIcons; set => _spellIcons = value; }
        public Dictionary<string, Sprite> Battlers { get => _battlers; set => _battlers = value; }
        public Dictionary<string, VolumeProfile> Profiles { get => _profiles; set => _profiles = value; }

        public ResourcesSystem()
        {
            LoadResources();
        }

        public List<T> LoadResource<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path).ToList();
        }

        public void LoadResources()
        {
            _miniatures = new List<Sprite>(Resources.LoadAll<Sprite>("Graphics/Miniatures/icons")); // Assuming "miniatureSheet" is the name of the sprite

            _clips = new Dictionary<string, AudioClip>();
            foreach(AudioClip clip in LoadResource<AudioClip>("Audio/Sfx")) _clips.Add(clip.name, clip);

            _locations = new Dictionary<string, string>();
            foreach(TextAsset content in LoadResource<TextAsset>("Locations")) _locations.Add(content.name, content.text);
            
            _spellIcons = new Dictionary<string, Sprite>();
            foreach(Sprite content in LoadResource<Sprite>("Graphics/SpellIcons")) _spellIcons.Add(content.name, content);

            _battlers = new Dictionary<string, Sprite>();
            foreach(Sprite content in LoadResource<Sprite>("Graphics/Battlers")) _battlers.Add(content.name.ToLower(), content);

            _profiles = new Dictionary<string, VolumeProfile>();
            foreach(VolumeProfile content in LoadResource<VolumeProfile>("Maps/Volume")) _profiles.Add(content.name, content);
        }

        public AudioClip LoadClip(string name)
        {
            if(Clips.TryGetValue(name, out AudioClip clip))
            {
                return clip;
            }
                
            ConsoleProDebug.LogAsType($"Clip by name {name} doesn't exist!", "Error");
            return Clips.Values.First();
        }

        public VolumeProfile LoadVolumeProfile(string name)
        {
            if(Profiles.TryGetValue(name, out VolumeProfile profile))
            {
                return profile;
            }
                
            ConsoleProDebug.LogAsType($"Clip by name {name} doesn't exist!", "Error");
            return Profiles.Values.First();
        }

        public Sprite LoadBattler(string name)
        {
            if(Battlers.TryGetValue(name.ToLower(), out Sprite profile))
            {
                return profile;
            }
                
            ConsoleProDebug.LogAsType($"Battler by name {name} doesn't exist!", "Error");
            return Battlers.Values.First();
        }
    }
}