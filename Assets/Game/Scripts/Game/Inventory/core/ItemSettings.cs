
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsForItem", menuName = "Inventory/NewItem", order = 1)]
public class ItemSettings : ScriptableObject
{
    public string NameOfItem = "SaveAsItem";
    public string Description = "";
    public int GUIDitem = 0;
    [SerializeField] protected List<Properties> allProperties = new List<Properties>();
    public List<Properties> GetAllProperties()
    {
        return allProperties;
    }

    // public void SaveToGlobal()
    // {
    //     WorldsSave.SaveToPath(this, NameOfSaveFile);
    // }

    // public ItemSettings Load()
    // {
    //     return WorldsSave.LoadFrom<ItemSettings>(NameOfSaveFile);
    // }
}
