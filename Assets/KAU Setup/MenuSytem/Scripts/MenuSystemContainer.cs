
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Services.MenuSystem
{
    [System.Serializable]
    public class MenuSystemContainer : ScriptableObject
    {
        [Tooltip("Assign the  Menu Prefab here")]
        public Menu MenuPrefab;

        /// <summary> Affect the Intantiation (when false, runs in coroutine)</summary>
        [Tooltip("Affect the Instantiation (when false, runs in coroutine).")]
        public bool InstantiateFast = false;

        /// <summary> Coroutine to wait for (only if InstantiateFast = false ) </summary>
        [HideInInspector] public Coroutine WorkingCoroutine;

#if UNITY_EDITOR
        /// <summary>
        ///	Use it to create new types (don't orget to set them up and place reference in prefab!)
        /// </summary>
        [MenuItem("Tools/MenuManager/CreateNewMenuItem")]
        public static void CreateScriptMenuSystemItem()
        {
            MenuSystemContainer customSriptableObject = ScriptableObject.CreateInstance<MenuSystemContainer>();
            AssetDatabase.CreateAsset(customSriptableObject, "Assets/UnassignedMenuType.asset"); // always .asset for arbitrary assets!
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = customSriptableObject;
        }
#endif

    }
}
