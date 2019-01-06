using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    public Item item;
    public uint itemCount;

    public InventorySlot()
    {
    }
    public InventorySlot(Item newItem, uint itemCount)
    {
        item = newItem;
        this.itemCount = itemCount;
    }

    public override string ToString()
    {
		return string.Format("InventorySlot {0} [{1}],.",item.ToString(),itemCount); 
    }
}
