using System.Collections;
using System.Collections.Generic;

public class InventoryItemStack
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItemStack(InventoryItemData source)
    {
        stackSize = 0;
        data = source;
        AddToStack();
    }

    public bool AddToStack()
    {
        if (stackSize < data.maxCapacity)
        {
            stackSize++;
            return true;
        }
        else
            return false;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
