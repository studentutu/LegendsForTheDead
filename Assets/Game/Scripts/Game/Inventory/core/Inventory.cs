using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] private Entity currentOwner = null;
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public Inventory(Entity owner)
        {
            currentOwner = owner;
        }

        public void AddItem(Item newItem)
        {
            AddItems(newItem, 1);
        }
        public void AddItems(Item newItem, uint itemCount)
        {
            Debug.Assert(itemCount > 0);

            InventorySlot slot = FindSlot(newItem);

            if (slot == null)
            {
                // if not, add a slot
                slots.Add(new InventorySlot(newItem, itemCount));
            }
            else
            {
                slot.itemCount += itemCount;
            }
        }
        public InventorySlot RemoveItem(Item newItem)
        {
            return RemoveItems(newItem, 1);
        }
        public InventorySlot RemoveItems(Item newItem)
        {
            uint slotIndex;
            InventorySlot slot = FindSlot(newItem, out slotIndex);

            Debug.Assert(slot != null);
            slots.RemoveAt((int)slotIndex);
            return slot;
        }
        public InventorySlot RemoveItems(Item newItem, uint itemCount)
        {
            uint slotIndex;
            InventorySlot slot = FindSlot(newItem, out slotIndex);

            Debug.Assert(slot != null);
            Debug.Assert(itemCount <= slot.itemCount);

            InventorySlot result = null;
            if (itemCount < slot.itemCount)
            {
                slot.itemCount -= itemCount;
                result = new InventorySlot(newItem, itemCount);
            }
            else 
            {
                slots.RemoveAt((int)slotIndex);
                result = slot;
            }
            return result;
        }

        private InventorySlot FindSlot(Item findItem, out uint slotIndex)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];

                if (slot.item.GUIDitem == findItem.GUIDitem)
                {
                    slotIndex = (uint)i;
                    return slot;
                }
            }

            slotIndex = 0;
            return null;
        }
        private InventorySlot FindSlot(Item findItem)
        {
            uint slotIndex;

            return FindSlot(findItem, out slotIndex);
        }

        public void SaveToGlobal()
        {
            WorldsSave.SaveToPath<Inventory>(this, currentOwner.Name + currentOwner.GUID.ToString());
        }

        public void Load()
        {
            var oldInventory = WorldsSave.LoadFrom<Inventory>(currentOwner.Name +currentOwner.GUID.ToString());
            if (oldInventory != null)
            {
                slots.Clear();
                foreach (var item in oldInventory.slots)
                {
                    slots.Add(item);
                    Debug.Log(string.Format( "<Color=Blue> Added Old Item : {0} </Color>",item.ToString() ));
                }
            }
        }
    }

}