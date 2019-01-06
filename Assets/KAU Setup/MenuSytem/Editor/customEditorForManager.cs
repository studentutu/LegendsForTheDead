using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Services.MenuSystem
{
    [CustomEditor(typeof(MenuManager), true)]
    [CanEditMultipleObjects]
    public class customEditorForManager : Editor
    {

        public override void OnInspectorGUI()
        {
            // target!
            base.OnInspectorGUI();
            serializedObject.Update();

            var newProp = serializedObject.FindProperty("Container");
            if (newProp == null)
            {
                Debug.LogWarning("Create or Add Scriptable Object for MenuSystem!");
                return;
            }
            if (newProp.objectReferenceValue != null)
            {
                GUILayout.BeginVertical("HelpBox");
                SerializedObject inner = new SerializedObject(newProp.objectReferenceValue);
                inner.Update();
                DrawDefaultProps(newProp.objectReferenceValue, inner);
                inner.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

        }

        private static void DrawDefaultProps(Object currObject, SerializedObject serialObject)
        {
            // List<SerializedProperty> allProps = new List<SerializedProperty>();
            List<string> allPropsss = new List<string>();
            WindowContainer myObj = currObject as WindowContainer;
            foreach (System.Reflection.FieldInfo fi in myObj.GetType().GetFields(
                        System.Reflection.BindingFlags.NonPublic |
                         System.Reflection.BindingFlags.Instance))
            {
                allPropsss.Add(fi.Name);
            }

            foreach (var item in allPropsss)
            {
                var something = serialObject.FindProperty(item);
                if (something != null)
                {
                    EditorGUILayout.PropertyField(something);
                    // allProps.Add(something);
                }
            }
        }
    }
}
