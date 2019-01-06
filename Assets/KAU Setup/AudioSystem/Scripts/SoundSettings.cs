
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KAUGamesLviv.Services.Audio
{
	[CreateAssetMenu(fileName = "SoundSettings", menuName = "Sound/Settings", order = 1)]
    public class SoundSettings : ScriptableObject
    {
        [Header(" Main Settings For Mixers and Setup ")]
        public int Sound_initial_number = 3;
        public AudioMixer MixerToUse;
        public MixerSettings MixerSettings;
        public static Dictionary<int,AudioMixerGroup> All_Mixer_Groups = new Dictionary<int, AudioMixerGroup>();

        public enum SoundMixerGroup
        {
            MENU = 0,
            MUSIC = 1,
            AMBIENT = 2,
            SFX = 3,
            RandomAMB = 4,
            Master = 5,
        }

    }
}
