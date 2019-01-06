
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KAUGamesLviv.Services.Audio
{
    [CreateAssetMenu(fileName = "MixerSettings", menuName = "Sound/MixerSettings", order = 3)]
    public class MixerSettings : ScriptableObject
    {
        [Header("All volumes for Mixer. Numeration as in SoundSettings.SoundMixerGroup.")]
        [SerializeField] public List<float> MixerVolumes = new List<float>();
    }
}
