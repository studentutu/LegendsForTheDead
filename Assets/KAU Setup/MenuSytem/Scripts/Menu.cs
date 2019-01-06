using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Services.MenuSystem
{
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        /// <summary>
        ///	 Each menu has the static field of type T =  Instance!
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }

        // public static  void Show();
        // public static   void Hide();

        /// <summary> 
        /// Always ask this to Create/Display/Call actual MenuType.
        /// Same Functionality as <see cref="OpenCoroutine(MenuType)"/> method.
        /// </summary>

        protected static void Open(MenuType menuType, bool instant = true)
        {
            if (Instance == null)
            {
                MenuManager.Instance.Container.CreateInstance(menuType);
            }
            else
            {
                Instance.gameObject.SetActive(true);
            }

            MenuManager.Instance.Container.OpenMenu(Instance);
        }

        /// <summary> 
        /// Override method for use of Coroutines.
        /// Same Functionality as <see cref="Open(MenuType,bool)"/> method.
        /// </summary>
        protected static IEnumerator OpenCoroutine(MenuType menuType)
        {
            bool instant = MenuManager.Instance.Container.CheckInstanceFast(menuType);
            if (instant)
            {
                Open(menuType, true);
                yield break;
            }

            if (Instance == null)
            {
                MenuManager.Instance.Container.CreateInstance(menuType);
            }
            else
            {
                Instance.gameObject.SetActive(true);
            }

            while (Instance == null)
            {
                yield return null;
            }
            MenuManager.Instance.Container.OpenMenu(Instance);
        }

        /// <summary>
        ///	Alwys ask/call to close the MenuType
        /// </summary>
        protected static void Close()
        {
            if (Instance == null)
            {
                WindowContainer.Log("Trying to close menu {0} but Instance is null" + typeof(T));
                if (MenuManager.Instance.Container._pausedMenus > 0)
                {
                    byte newPausedMenus = 0;
                    List<Menu> actualMenus = new List<Menu>();
                    for (int i = 0; i < MenuManager.Instance.Container.MenuStack.Count; i++)
                    {
                        if (MenuManager.Instance.Container.MenuStack[i] == null)
                            continue;
                        switch (MenuManager.Instance.Container.MenuStack[i].MenuType)
                        {
                            /*
                            *    Add your cases here: ALL_MENUS, MenuManagerSO
                            *
                            **/
                            case MenuType.MainMenu:
                                newPausedMenus++;
                                break;
                            case MenuType.Settings:
                                newPausedMenus++;
                                break;
                            case MenuType.GameUI:

                                break;
                            case MenuType.Shop:
                                newPausedMenus++;
                                break;
                        }
                        actualMenus.Add(MenuManager.Instance.Container.MenuStack[i]);
                    }

                    if (newPausedMenus > 0)
                    {
                        MenuManager.Instance.Container.Paused = true;
                    }
                    else
                    {
                        MenuManager.Instance.Container.Paused = false;
                    }
                    MenuManager.Instance.Container._pausedMenus = newPausedMenus;
                }
                return;
            }

            MenuManager.Instance.Container.CloseMenu(Instance);
        }

        /// <summary>
        ///	Used to close the Menu
        /// </summary>
        public override void OnBackPressed()
        {
            Close();
        }
    }

    [System.Serializable]
    public abstract class Menu : MonoBehaviour
    {
        [HideInInspector] public bool isInitialized = false;
        [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
        public bool DestroyWhenClosed = true;

        [Tooltip("Disable menus that are under this one in the stack")]
        public bool DisableMenusUnderneath = true;

        [Tooltip("This Menu will Always be on top")]
        public bool AlwaysOnTop = true;

        [Tooltip("Assign Menu Type")]
        public MenuType MenuType = MenuType.Settings;
        public Canvas MyCanvas;

        [SerializeField]
        private MenuSystemContainer _myScriptableObject;
        public abstract void OnBackPressed();

    }


}
