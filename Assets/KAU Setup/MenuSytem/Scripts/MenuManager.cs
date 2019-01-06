
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.MenuSystem
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
        [Header("MenuManager container of types here (add types):")]
        public WindowContainer Container;
        private static bool initialized = false;
        private void Awake()
        {
            if (Instance == null && !initialized)
            {
                initialized = true;
                SceneManager.sceneLoaded += ClearAllMenus;
            }
            Instance = this;
        }
        private static void ClearAllMenus(Scene scene, LoadSceneMode mode)
        {
            if (Instance == null)
            {
                return;
            }
            if (mode == LoadSceneMode.Single)
            {
                int newPausedMenus = 0;
                System.Collections.Generic.List<Menu> actualMenus = new System.Collections.Generic.List<Menu>();
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

        }


        /// <summary>
        /// Used to open Menus.
        /// Also, you can use just CustomMenuType.Show()
        /// </summary>

        public void OpenMenuManual(int menuType)
        {
            switch (menuType)
            {
                case 1:
                    // MenuType.Shop   
                    ShopMenuType.Show();
                    break;
            }
        }
    }
}
