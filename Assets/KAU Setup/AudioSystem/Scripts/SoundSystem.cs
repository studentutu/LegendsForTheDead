using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ForChangingScenes
using UnityEngine.SceneManagement;
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KAUGamesLviv.Services.Audio
{
    [RequireComponent(typeof(AudioListener))]
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private SoundSettings Settings;
        private static SoundSystem _instance = null;
        [HideInInspector] private List<SoundEmitter> sound_Emitters;

        #region Helping_Functions

        // For Each Group We need to make it's cuttOf Volume
        private static float SOUND_VOL_CUTOFF = 60.0f;
        private static float SOUND_AMP_CUTOFF = 0.008f;  // min Volume to play [ 0 - 1]

        private Dictionary<int, float> MaxVolumeFromMixerGroup = new Dictionary<int, float>();

        /// <param name="amplitude" > Takes float from [0-1] </param> 
        // Returns [-60 - MaxVolumeOnGroup]
        private static float DecibelFromAmplitude(SoundSettings.SoundMixerGroup groupToCalculateFor, float amplitude)
        {
            if (amplitude < SOUND_AMP_CUTOFF)
                return -SOUND_VOL_CUTOFF;

#if UNITY_EDITOR
            if (_instance == null)
            {
                var allMixersGUID = AssetDatabase.FindAssets("t:AudioMixer");
                string pathTo = AssetDatabase.GUIDToAssetPath(allMixersGUID[0]);
                var floatFromMixer = AssetDatabase.LoadAssetAtPath(pathTo, typeof(UnityEngine.Audio.AudioMixer)) as UnityEngine.Audio.AudioMixer;
                float currentValue = 0;
                floatFromMixer.GetFloat("volume" + groupToCalculateFor.ToString(), out currentValue);
                return (amplitude * (currentValue + SOUND_VOL_CUTOFF)) - SOUND_VOL_CUTOFF;
            }
#endif

            return (amplitude * (_instance.MaxVolumeFromMixerGroup[(int)groupToCalculateFor] + SOUND_VOL_CUTOFF)) - SOUND_VOL_CUTOFF;
        }

        /// <param name="amplitude" > Takes float from [-60 - 0] </param> 
        // Returns [0-1]
        private static float AmplitudeFromDecibel(SoundSettings.SoundMixerGroup groupToCalculateFor, float decibel)
        {
            if (decibel <= -SOUND_VOL_CUTOFF)
            {
                return 0;
            }
#if UNITY_EDITOR
            if (_instance == null)
            {
                var allMixersGUID = AssetDatabase.FindAssets("t:AudioMixer");
                string pathTo = AssetDatabase.GUIDToAssetPath(allMixersGUID[0]);
                var floatFromMixer = AssetDatabase.LoadAssetAtPath(pathTo, typeof(UnityEngine.Audio.AudioMixer)) as UnityEngine.Audio.AudioMixer;
                float currentValue = 0;
                floatFromMixer.GetFloat("volume" + groupToCalculateFor.ToString(), out currentValue);
                return ((decibel + SOUND_VOL_CUTOFF) / (currentValue + SOUND_VOL_CUTOFF));
            }
#endif
            return ((decibel + SOUND_VOL_CUTOFF) / (_instance.MaxVolumeFromMixerGroup[(int)groupToCalculateFor] + SOUND_VOL_CUTOFF));
        }
        #endregion

        #region Private Core For SoundSystem

        // These are internal to the SoundSystem
        public class SoundEmitter
        {
            public AudioSource source;
            public SoundDefinition soundDef;
            public Interpolator interpolateTo = new Interpolator(0, Interpolator.CurveType.None);
            public bool playing
            {
                get
                {
                    if (source == null)
                        return false;
                    return source.isPlaying;
                }
            }

            public void Kill()
            {
                if (source != null)
                {
                    source.Stop();
                    source.loop = false;
                    source.volume = 1;
                }
                soundDef = null;
                interpolateTo.Stop();

            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Init();
            }

        }

        private void OnDestroy()
        {

            foreach (var item in sound_Emitters)
            {
                item.Kill();
            }
            SceneManager.activeSceneChanged -= FreeUnusedEmmiters;
        }

        private const string ALLCONfigs = "SOUND_MIXER_CONFIGS";
        private void Init()
        {
            if (Settings == null)
            {
                Debug.LogError(" You have not Provided your Settings for Audio System");
                return;
            }
            if (Settings.MixerSettings == null)
            {
                Debug.LogError(" You have not Provided MixerSettings for Settings in Audio System");
                return;
            }
            // Create pool of emitters
            sound_Emitters = new List<SoundEmitter>(Settings.Sound_initial_number);
            for (var i = 0; i < Settings.Sound_initial_number; i++)
            {
                var source = MakeAudioSource(i);
                var emitter = new SoundEmitter();
                emitter.source = source;
                emitter.Kill();
                sound_Emitters.Add(emitter);
            }

            // Set up mixer groups
            SoundSettings.All_Mixer_Groups.Clear();
            MaxVolumeFromMixerGroup.Clear();
            var mixerVolumes = Settings.MixerSettings;
            int number = System.Enum.GetNames(typeof(SoundSettings.SoundMixerGroup)).Length;

            for (int i = 0; i < number; i++)
            {

#if UNITY_EDITOR
                if (Settings.MixerToUse.FindMatchingGroups(((SoundSettings.SoundMixerGroup)i).ToString())[0] == null)
                {
                    Debug.LogError(" Used Mixer does not have submixer : " + ((SoundSettings.SoundMixerGroup)i).ToString());
                }
#endif
                SoundSettings.All_Mixer_Groups.Add(i, Settings.MixerToUse.FindMatchingGroups(((SoundSettings.SoundMixerGroup)i).ToString())[0]);
                float returnValueInDecibels = mixerVolumes.MixerVolumes[i];
                MaxVolumeFromMixerGroup.Add(i, returnValueInDecibels);

            }

            // Clean-up on Changed Scenes
            SceneManager.activeSceneChanged += FreeUnusedEmmiters;
        }

        private void FreeUnusedEmmiters(Scene current, Scene next)
        {
            foreach (var item in sound_Emitters)
            {
                if (item.soundDef != null)
                {

                    switch (item.soundDef.soundGroup)
                    {
                        case SoundSettings.SoundMixerGroup.SFX:
                            item.Kill();
                            break;
                        case SoundSettings.SoundMixerGroup.AMBIENT:
                            item.Kill();
                            break;
                    }
                }
                else
                {
                    item.Kill();
                }
            }
        }

        private AudioSource MakeAudioSource(int number)
        {
            var go = new GameObject("SoundSystemSource" + number);
            go.transform.parent = this.transform;
            return go.AddComponent<AudioSource>();
        }

        private SoundEmitter AllocEmitter()
        {
            // Look for unused emitter
            foreach (var e in sound_Emitters)
            {
                if (!e.playing)
                {
                    return e;
                }
            }

            // New emitter is needed
            var source = MakeAudioSource(sound_Emitters.Count);
            var emitter = new SoundEmitter();
            emitter.source = source;

            sound_Emitters.Add(emitter);
            return emitter;
        }

        private void StartEmitter(SoundEmitter emitter)
        {
            var soundDef = emitter.soundDef;
            var source = emitter.source;

            StartSource(source, soundDef, emitter);
        }

        // Here are all manipulation with Mixers will take place 
        private static void StartSource(AudioSource source, SoundDefinition soundDef, SoundEmitter emmiter = null)
        {
            source.clip = soundDef.clips[Random.Range(0, soundDef.clips.Count)];

            // Map from halftone space to linear playback multiplier
            if (soundDef.pitchMin == 0 && soundDef.pitchMax == 0)
            {
            }
            else
            {
                // This is a formula to translate the Decibel Pitsh into pitch for SoundEmitor
                source.pitch = Mathf.Pow(2.0f, Random.Range(soundDef.pitchMin, soundDef.pitchMax) / 12.0f);
            }

            source.volume = AmplitudeFromDecibel(soundDef.soundGroup, soundDef.volumeDecibels);
            source.loop = soundDef.loop;

#if UNITY_EDITOR
            if (_instance == null)
            {
                var allMixersGUID = AssetDatabase.FindAssets("t:AudioMixer");
                UnityEngine.Audio.AudioMixerGroup finalMixer = null;
                foreach (var item in allMixersGUID)
                {
                    string pathTo = AssetDatabase.GUIDToAssetPath(item);
                    var Mixer = AssetDatabase.LoadAssetAtPath(pathTo, typeof(UnityEngine.Audio.AudioMixer)) as UnityEngine.Audio.AudioMixer;
                    if (finalMixer == null)
                    {
                        finalMixer = Mixer.FindMatchingGroups("Master")[0];
                    }
                    var someGroup = Mixer.FindMatchingGroups(soundDef.soundGroup.ToString())[0];
                    if (someGroup != null)
                    {
                        finalMixer = someGroup;
                        break;
                    }
                }


                source.outputAudioMixerGroup = finalMixer;

                source.Play();
                return;
            }
#endif
            source.outputAudioMixerGroup = SoundSettings.All_Mixer_Groups[(int)soundDef.soundGroup];

            if (soundDef.InterpolateType == Interpolator.CurveType.None || emmiter == null)
            {
                source.Play();
            }
            else
            {
                source.Play();
                source.volume = 0.06f;
                emmiter.interpolateTo.SetStartValue(0.06f, soundDef.InterpolateType);
                emmiter.interpolateTo.SetFinalValuesAndStart(1, soundDef.OnStartSoundInterpolateTime);
            }

        }
        #endregion

        #region  Public Method
#if UNITY_EDITOR
        /// <param name="whichSound" > Takes SoundDefinition </param> 
        public static void StartSoundEditor(UnityEngine.AudioSource source, SoundDefinition whichSound)
        {
            StartSource(source, whichSound);
        }

#endif  
        public static bool IsPlaying(SoundDefinition currentDef)
        {
            if (currentDef.emmiter == null)
                return false;

            if (currentDef.emmiter.soundDef == currentDef)
                return currentDef.emmiter.playing;

            return false;

        }

        /// <param name="whichSound" > Takes SoundDefinition </param> 
        public static void StartSound(SoundDefinition whichSound)
        {
            StartSound(whichSound, Vector3.zero);
        }
        public static void StartSound(SoundDefinition whichSound, Vector3 position)
        {
            if (!whichSound.Duplication && isAnyPlaying(whichSound))
                return;

            var emitter = _instance.AllocEmitter();
            emitter.soundDef = whichSound;
            _instance.StartEmitter(emitter);
            emitter.source.transform.position = position;
            whichSound.emmiter = emitter;
        }

        public static void StartSound(SoundDefinition whichSound, Transform parent)
        {
            if (!whichSound.Duplication && isAnyPlaying(whichSound))
                return;

            var emitter = _instance.AllocEmitter();
            emitter.soundDef = whichSound;
            _instance.StartEmitter(emitter);
            emitter.source.transform.SetParent(parent, false);
            emitter.source.transform.position = parent.position;
            whichSound.emmiter = emitter;
        }

        /// <param name="whichSound" > Takes SoundDefinition </param> 
        public static void StopSound(SoundDefinition whichSoundEmmiter)
        {

            if (whichSoundEmmiter == null || whichSoundEmmiter.emmiter == null) return;
            if (whichSoundEmmiter.emmiter.soundDef == whichSoundEmmiter)
            {
                switch (whichSoundEmmiter.emmiter.interpolateTo.InterpolateType)
                {
                    case Interpolator.CurveType.None:
                        whichSoundEmmiter.emmiter.Kill();
                        break;
                    default:
                        whichSoundEmmiter.emmiter.interpolateTo.SetStartValue(whichSoundEmmiter.emmiter.source.volume, whichSoundEmmiter.InterpolateType);
                        whichSoundEmmiter.emmiter.interpolateTo.SetFinalValuesAndStart(0, whichSoundEmmiter.OnStopSoundInterpolateTime);
                        break;
                }
            }
        }
        public static void StopAllSounds(SoundDefinition whichSoundEmmiter)
        {
            if (whichSoundEmmiter == null) return;
            foreach (var item in _instance.sound_Emitters)
            {
                if (item.soundDef == null)
                    continue;
                if (item.soundDef == whichSoundEmmiter && item.source.isPlaying)
                {
                    item.Kill();
                }
            }
        }
        public static void KillAllSoundsByMixer(SoundSettings.SoundMixerGroup whichMixerGroup)
        {
            foreach (var item in _instance.sound_Emitters)
            {
                if (item.source == null)
                    continue;
                if (item.source.isPlaying && item.source.outputAudioMixerGroup == SoundSettings.All_Mixer_Groups[(int)whichMixerGroup])
                {
                    item.Kill();
                }
            }
        }

        public static bool isAnyPlaying(SoundDefinition whichSoundEmmiter)
        {
            bool result = false;
            foreach (var item in _instance.sound_Emitters)
            {
                if (item.soundDef == null)
                    continue;

                result |= item.soundDef == whichSoundEmmiter && item.source.isPlaying;
            }
            return result;
        }


        /// <param name="newVolume" > Takes float from [0 - 1] </param> 
        public static void SetVolume(SoundSettings.SoundMixerGroup whichGroup, float newVolume)
        {
            float decibels = DecibelFromAmplitude(whichGroup, newVolume);
            if (decibels <= -SOUND_VOL_CUTOFF)
                decibels = -80f;
            SoundSettings.All_Mixer_Groups[(int)whichGroup].audioMixer.SetFloat("volume" + whichGroup.ToString(), decibels);
        }

        /// <param name="whichGroup" >  Takes group to retrieve it's volume </param> 
        /// <returns> Returns Decibels [0, 1].</returns>
        public static float GetVolume(SoundSettings.SoundMixerGroup whichGroup)
        {
            float returnValue = 0;
            SoundSettings.All_Mixer_Groups[(int)whichGroup].audioMixer.GetFloat("volume" + whichGroup.ToString(), out returnValue);

            float result = AmplitudeFromDecibel(whichGroup, returnValue);
            return result;
        }


        #endregion

        private void Update()
        {
            // Update running sounds
            int uniqNumber = -1;
            foreach (var e in sound_Emitters)
            {
                uniqNumber++;
                if (!e.playing)
                {
                    continue;
                }
                if (e.source == null)
                {
                    // Could happen if parent was killed. Not good, but fixable:
                    e.source = MakeAudioSource(uniqNumber);
                }

                if (e.interpolateTo.IsMoving())
                {
                    float clampValues = e.interpolateTo.GetValue();
                    if (clampValues <= 0.05f)
                    {
                        clampValues = 0;
                    }
                    if (clampValues >= 0.95f)
                    {
                        clampValues = 1;
                    }
                    e.source.volume = clampValues;
                }

                // Only if the interpolateTo was towards the negative, ensure that is stops!
                if (e.source.volume == 0)
                {
                    e.Kill();
                }
            }
        }



    }

}

