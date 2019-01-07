using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace Services.DependencyInjection
{

    [CustomEditor(typeof(IDependencyContext), true)]
    [CanEditMultipleObjects]
    public class customEditorDepOnContext : Editor
    {
        private Dictionary<string, SerializedProperty> allPropsDict;
        private List<CustomProperty> allMyDeps = new List<CustomProperty>();
        struct CustomProperty
        {
            public SettingsForType _myTypeSettings;

        }
        public void OnEnable()
        {
            var _bundleDep = target as IDependencyContext;
            var allProps = new List<string>();
            Dictionary<string, SerializedProperty> allProperties = new Dictionary<string, SerializedProperty>();
            var myObj = _bundleDep;
            if (myObj != null)
            {
                allProps.AddRange
                    (myObj.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(fieldInfo => fieldInfo.Name));
            }
            foreach (var item in allProps)
            {
                var something = serializedObject.FindProperty(item);
                UnityEngine.Debug.Log(" Found Propterty : " + item);
                if (!allProperties.ContainsKey(item) && something != null)
                {
                    allProperties.Add(item, something);
                }
            }
            allPropsDict = allProperties;

        }
        public override void OnInspectorGUI()
        {
            // target!
            // base.OnInspectorGUI();
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "allDependencies" });
            SerializedProperty newProp = null;
            allPropsDict.TryGetValue("allDependencies", out newProp);

            // if (newProp != null && newProp.objectReferenceValue != null)
            // {
            //     GUILayout.BeginVertical("HelpBox");
            //     SerializedObject inner = new SerializedObject(newProp.objectReferenceValue);
            //     inner.Update();


            //     DrawDefaultProps(newProp.objectReferenceValue, inner);
            //     inner.ApplyModifiedProperties();
            //     GUILayout.EndVertical();
            // }

            serializedObject.ApplyModifiedProperties();

        }

        private static void DrawDefaultProps(Object currObject, SerializedObject serialObject)
        {
            List<string> allPropsss = new List<string>();
            DependenciesSettings myObj = currObject as DependenciesSettings;
            foreach (System.Reflection.FieldInfo fi in myObj.GetType().GetFields(
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.Public))
            {
                allPropsss.Add(fi.Name);
            }

            foreach (var item in allPropsss)
            {
                var something = serialObject.FindProperty(item);
                if (something != null)
                {
                    EditorGUILayout.PropertyField(something);
                }
            }
        }
    }
}
