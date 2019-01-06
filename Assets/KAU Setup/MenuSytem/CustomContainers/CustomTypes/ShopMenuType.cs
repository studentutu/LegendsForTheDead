
using Services.MenuSystem;
public class ShopMenuType : Menu<ShopMenuType>
{
    #region Always Here
    public static void Hide()
    {
        // Your Logic Here

        // Always Ask to close
        Close();
    }

    public static void Show()
    {
        // Your Logic Here

        // Always use one of the Following to Open
        // Static Method
        Open(MenuType.Shop);

        // Or Corotuine
        // MenuManager.Instance.StartCoroutine(Open(MenuType.Shop));
    }
    #endregion

    #region  Custom Fields,Functions etc.
    public void ShowMe()
    {
        Show();
    }
    public void HideMe()
    {
        Hide();
    }
    #endregion



}
