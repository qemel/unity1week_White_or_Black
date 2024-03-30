using System;
using UnityEngine;

namespace u1w_2024_3.Src.Service
{
    [CreateAssetMenu(menuName = "u1w 2024/SoundRepository")]
    public sealed class SoundRepository : ScriptableObject
    {
        [Serializable]
        public class SoundData
        {
            public string Name => _name;
            public AudioClip Clip => _clip;

            [SerializeField] private string _name;
            [SerializeField] private AudioClip _clip;
        }

        public SoundData[] Sounds => _sounds;

        [SerializeField] private SoundData[] _sounds;
        
        public AudioClip GetClip(string clipName)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Name == clipName)
                {
                    return sound.Clip;
                }
            }

            return null;
        }
    }
}