using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Services.MenuSystem
{
    [CustomEditor(typeof(Menu), true)]
    [CanEditMultipleObjects]
    public class customEditorForMenu : Editor
    {
        SerializedProperty mySerializedProp;

        public override void OnInspectorGUI()
        {
            // target!
            base.OnInspectorGUI();
            serializedObject.Update();
            mySerializedProp = serializedObject.FindProperty("_myScriptableObject");
            if (mySerializedProp.objectReferenceValue == null)
                return;
            SerializedObject inner = new SerializedObject(mySerializedProp.objectReferenceValue);
            inner.Update();
            var obj = inner.FindProperty("InstantiateFast");

            if (obj != null)
            {
                Undo.RecordObject(mySerializedProp.objectReferenceValue, "Change Menu Prefab");
                GUILayout.BeginVertical("HelpBox");

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUI.skin.toggle.onActive.textColor = Color.blue;
                EditorGUILayout.LabelField(obj.displayName, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                obj.boolValue = EditorGUILayout.Toggle(obj.boolValue, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                // Change Prefab
                var anotherProperty = obj.serializedObject.FindProperty("menuPrefab");
                if (anotherProperty != null)
                {

                    if (anotherProperty.objectReferenceValue != null)
                        Undo.RecordObject(anotherProperty.objectReferenceValue, "Change Menu From Game Prefab");

                    EditorGUILayout.PropertyField(anotherProperty);
                }

                // Apply to inner Object!
                inner.ApplyModifiedProperties();

                GUILayout.EndVertical();

            }
            // Apply to outer Object!            
            serializedObject.ApplyModifiedProperties();
        }


    }
}
