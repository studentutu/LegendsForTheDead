using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    private const string ItemConst = "UItem";
    private List<Properties> _myProperties = new List<Properties>();
    public int GUIDitem = 0;
    public string NameOfItem = " Generic Item";
    public string Description;
    public Item()
    {
        GUIDitem = GlobalIds.GenerateGUID();
        NameOfItem = GUIDitem.ToString();
    }
    public Item(ItemSettings settingsToUse)
    {
        _myProperties.Clear();
        var FromSettings = settingsToUse.GetAllProperties();
        var allProprs = new Properties[FromSettings.Count];
        FromSettings.CopyTo(allProprs);
        _myProperties.AddRange(allProprs);
        GUIDitem = settingsToUse.GUIDitem;
        NameOfItem = settingsToUse.NameOfItem;
        Description = settingsToUse.Description;
    }

    public override string ToString()
    {
        return string.Format("-{0}-, ID:{1}, ",NameOfItem, GUIDitem,_myProperties.ToString());
    }

    public void SaveToGlobal()
    {
        WorldsSave.SaveToPath<Item>(this, ItemConst + GUIDitem.ToString());
    }

    public Item Load()
    {
        return WorldsSave.LoadFrom<Item>(ItemConst + GUIDitem.ToString());
    }
}


