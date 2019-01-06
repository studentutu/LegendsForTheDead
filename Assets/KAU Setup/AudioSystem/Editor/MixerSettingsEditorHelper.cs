using UnityEngine;
using UnityEditor;
using KAUGamesLviv.Services.Audio;

[CustomEditor(typeof(MixerSettings), true)]
[CanEditMultipleObjects]
public class MixerSettingsEditorHelper : Editor
{

    public override void OnInspectorGUI()
    {

        var mixerSet = (MixerSettings)target;
        if (mixerSet.MixerVolumes.Count == 0 || GUILayout.Button(" Validate Mixer Groups again"))
        {
            ValidateMixer(mixerSet);
            EditorUtility.SetDirty(mixerSet);
            AssetDatabase.SaveAssets();
            serializedObject.Update();
        }
        DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

        serializedObject.ApplyModifiedProperties();
    }
    private static void ValidateMixer(MixerSettings toValidate)
    {
        var allMixersGUID = AssetDatabase.FindAssets("t:AudioMixer");

        string pathTo = AssetDatabase.GUIDToAssetPath(allMixersGUID[0]);
        var Mixer = AssetDatabase.LoadAssetAtPath(pathTo, typeof(UnityEngine.Audio.AudioMixer)) as UnityEngine.Audio.AudioMixer;
        int number = System.Enum.GetNames(typeof(SoundSettings.SoundMixerGroup)).Length;
        toValidate.MixerVolumes.Clear();
        float[] newValuesDecibels = new float[number];
        Debug.Log(" Using Mixer : " + pathTo);
        for (int i = 0; i < number; i++)
        {
            if (Mixer.FindMatchingGroups(((SoundSettings.SoundMixerGroup)i).ToString())[0] == null)
            {
                Debug.LogWarning("Mixer " + pathTo + " does not have Mixer Group : " + ((SoundSettings.SoundMixerGroup)i).ToString());
                newValuesDecibels[i] = 0;
                continue;
            }
            float returnValueInDecibels = 0;
            Mixer.GetFloat("volume" + ((SoundSettings.SoundMixerGroup)i).ToString(), out returnValueInDecibels);
            newValuesDecibels[i] = returnValueInDecibels;
        }
        toValidate.MixerVolumes.AddRange(newValuesDecibels);
    }
}
