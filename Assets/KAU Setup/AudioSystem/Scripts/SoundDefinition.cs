using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAUGamesLviv.Services.Audio
{
    [CreateAssetMenu(fileName = "SoundDefinitionFor", menuName = "Sound/Definition", order = 2)]
    public class SoundDefinition : ScriptableObject
    {
        public SoundSystem.SoundEmitter emmiter;
        public List<AudioClip> clips = new List<AudioClip>();

        [Header("Interpolate Values")]
        public Interpolator.CurveType InterpolateType = Interpolator.CurveType.None;
        public float OnStartSoundInterpolateTime = 1f;
        public float OnStopSoundInterpolateTime = 1f;

        [Header(" Additional Volume Settings")]
        [Range(-60.0f, 20f)]
        public float volumeDecibels = 0f;

        [Range(-20, 20.0f)]
        public float pitchMin = 0.0f;

        [Range(-20, 20.0f)]
        public float pitchMax = 0.0f;

        [Header("Looping")]
        public bool loop = false;

        [Header("Mixer Group for this Sound")]
        public SoundSettings.SoundMixerGroup soundGroup;

        [Header("Allow duplication of Sound")]
        public bool Duplication = false;

    }
}
