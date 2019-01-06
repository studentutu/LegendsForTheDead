using System.Collections.Generic;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Services.MenuSystem
{
    [System.Serializable]
    public class WindowContainer : ScriptableObject
    {
        #region MenuTypes
        /*
        *    Add your cases here: ALL_MENUS, MenuManagerSO
        *
        **/
        [SerializeField] private MenuSystemContainer _settingsMenuType;
        [SerializeField] private MenuSystemContainer _mainMenuMenuType;

        [SerializeField] private MenuSystemContainer _GameUIMenuType;

        [SerializeField] private MenuSystemContainer _shopMenuType;
        [SerializeField] private MenuSystemContainer _noConnectionMenuType;
        #endregion


        // [HideInInspector]
        public bool Paused = false;
        [HideInInspector] public List<Menu> MenuStack = new List<Menu>();
        public int _pausedMenus
        {
            get
            {
                return _privatePausedMenus;
            }
            set
            {
                if (value < 0)
                {
                    _privatePausedMenus = 0;
                }
                else
                {
                    _privatePausedMenus = value;
                }
            }
        }
        private int _privatePausedMenus = 0;

        public static void Log(string message)
        {
            Debug.Log(message);
        }

        /// <summary>
        ///	DO not use directly! Gets called from the Instance of menu!
        /// </summary>
        public bool CheckInstanceFast(MenuType menuType)
        {
            switch (menuType)
            {
                /*
                *    Add your cases here: ALL_MENUS, MenuManagerSO
                *
                **/
                case MenuType.MainMenu:
                    return _mainMenuMenuType.InstantiateFast;
                case MenuType.Settings:
                    return _settingsMenuType.InstantiateFast;
                case MenuType.GameUI:
                    return _GameUIMenuType.InstantiateFast;
                case MenuType.Shop:
                    return _shopMenuType.InstantiateFast;
            }

            Log("Please add cases to MenuManager!");
            return true;
        }

        /// <summary>
        ///	DO not use directly! Gets called from the Instance of menu!
        /// </summary>
        public void CreateInstance(MenuType menuType)
        {
            switch (menuType)
            {
                /*
                *    Add your cases here: ALL_MENUS, MenuManagerSO
                *
                **/
                case MenuType.MainMenu:
                    Instantiate(_mainMenuMenuType.MenuPrefab);
                    break;
                case MenuType.Settings:
                    Instantiate(_settingsMenuType.MenuPrefab);
                    break;
                case MenuType.GameUI:
                    Instantiate(_GameUIMenuType.MenuPrefab);
                    break;
                case MenuType.Shop:
                    Instantiate(_shopMenuType.MenuPrefab);
                    break;
            }

        }

        /// <summary>
        ///	DO not use directly! Gets called from the Instance of menu!
        /// </summary>
        public void OpenMenu(Menu instance)
        {
            switch (instance.MenuType)
            {
                case MenuType.MainMenu:
                    _pausedMenus++;
                    break;
                case MenuType.Settings:
                    _pausedMenus++;
                    break;
                case MenuType.Shop:
                    _pausedMenus++;
                    break;
                default:
                    break;
            }

            Paused = _pausedMenus > 0;


            // De-activate top menu
            if (instance.DisableMenusUnderneath)
            {
                for (int i = MenuStack.Count - 1; i >= 0; i--)
                {
                    if (MenuStack[i] == null)
                        continue;

                    MenuStack[i].gameObject.SetActive(false);

                    // disable all till another DisableMenusUnderneath
                    if (MenuStack[i].DisableMenusUnderneath)
                        break;
                }
            }

            MenuStack.Add(instance);
            // To Be always sure of menu - refill the List
            List<Menu> temporalMenus = new List<Menu>();
            int sortOrderForTops = 0;
            foreach (var menu in MenuStack)
            {
                if (menu == null)
                    continue;

                if (!menu.AlwaysOnTop)
                {
                    sortOrderForTops++;
                }
                temporalMenus.Add(menu);
            }

            int sortOrdinary = 0;
            foreach (var menu in temporalMenus)
            {
                if (menu.AlwaysOnTop)
                {
                    // Always on top
                    menu.MyCanvas.sortingOrder = sortOrderForTops;
                    sortOrderForTops++;
                }
                else
                {
                    // Ordinary
                    menu.MyCanvas.sortingOrder = sortOrdinary;
                    sortOrdinary++;
                }

            }

            MenuStack.Clear();
            MenuStack.AddRange(temporalMenus);
            instance.isInitialized = true;
        }

        /// <summary>
        ///	DO not use directly! Gets called from the Instance of menu!
        /// </summary>
        public void CloseMenu(Menu intance)
        {
            switch (intance.MenuType)
            {
                /*
                *    Add your cases here: ALL_MENUS, MenuManagerSO
                **/
                case MenuType.MainMenu:
                    _pausedMenus--;
                    break;
                case MenuType.Settings:
                    _pausedMenus--;
                    break;
                case MenuType.Shop:
                    _pausedMenus--;
                    break;
            }

            if (_pausedMenus > 0)
            {
                Paused = true;
            }
            else
            {
                Paused = false;
            }

            if (MenuStack.Count == 0)
            {
                CloseGivenMenu(intance);
                return;
            }

            List<Menu> tempMenu = new List<Menu>();
            foreach (var menu in MenuStack)
            {
                if (menu == null || menu == intance)
                    continue;

                tempMenu.Add(menu);
            }
            MenuStack.Clear();
            MenuStack.AddRange(tempMenu);
            CloseGivenMenu(intance);
        }

        /// <summary>
        ///	DO not use directly! Gets called from the Instance of menu!
        /// </summary>
        private void CloseGivenMenu(Menu instance)
        {

            if (instance.DestroyWhenClosed)
            {
                // Addressables.ReleaseInstance(instance.gameObject);
                Destroy(instance.gameObject);
            }
            else
            {
                instance.gameObject.SetActive(false);
                instance.isInitialized = false;
            }

            // Re-activate top menu
            // If the re-activated menu is an overlay we need to activate the menu under it
            for (int i = MenuStack.Count - 1; i >= 0; i--)
            {
                if (MenuStack[i] == null)
                    continue;

                MenuStack[i].gameObject.SetActive(true);

                // disable all till another DisableMenusUnderneath
                if (MenuStack[i].DisableMenusUnderneath)
                    break;
            }

        }

        /// <summary>
        ///	use directly
        /// </summary>
        public void OnBackPressed()
        {
            for (int i = MenuStack.Count - 1; i >= 0; i--)
            {
                if (MenuStack[i] == null)
                    continue;

                MenuStack[i].OnBackPressed();
                break;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///	Used only once to create MenuManager ScriptableObject
        /// </summary>
        [MenuItem("Tools/MenuManager/CreateManager")]
        public static void CreateScriptMenuSystemItem()
        {
            WindowContainer customSriptableObject = ScriptableObject.CreateInstance<WindowContainer>();
            AssetDatabase.CreateAsset(customSriptableObject, "Assets/SOMenuManager.asset"); // always .asset for arbitrary assets!
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = customSriptableObject;
        }
#endif
    }

}