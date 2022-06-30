using System.Collections;
using System.Collections.Generic;

public class InventoryItemStack
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItemStack(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;

    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
