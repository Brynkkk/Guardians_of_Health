using System;
using UnityEngine;

[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData newItemData)
    {
        data = newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
