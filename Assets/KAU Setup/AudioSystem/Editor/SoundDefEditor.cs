using UnityEngine;
using UnityEditor;
using KAUGamesLviv.Services.Audio;

[CustomEditor(typeof(SoundDefinition),true)]
[CanEditMultipleObjects]
public class SoundDefEditor : Editor
{
    private static AudioSource testSource = null;

	
    public override void OnInspectorGUI()
    {
        if (testSource == null)
        {
            var go = new GameObject("testSource");
            go.hideFlags = HideFlags.HideAndDontSave; // will not save into 
            testSource = go.AddComponent<AudioSource>();
        }
        var sd = (SoundDefinition)target;

        // Allow playing audio even when sounddef is readonly
        var oldEnabled = GUI.enabled;
        GUI.enabled = true;
        if(testSource.isPlaying && GUILayout.Button("Stop []"))
        {
            testSource.Stop();
        }
        else if(!testSource.isPlaying && GUILayout.Button("Play >"))
        {
            SoundSystem.StartSoundEditor(testSource, sd);
        }
        GUI.enabled = oldEnabled;

        DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

        serializedObject.ApplyModifiedProperties();
    }
}