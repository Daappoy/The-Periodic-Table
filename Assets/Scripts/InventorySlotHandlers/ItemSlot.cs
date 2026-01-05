using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType
    {
        Weapon,
        Element
    }
    public int itemLimit = 1;
    public int currentItemCount = 0;
    public bool isFull = false;
    public DragDrop currentItem;
    public Elemen currentElement; // Element yang sedang disimpan di slot ini

    public void RemoveItem()
    {
        isFull = false;
        if (currentItemCount > 0)
        {
            currentItemCount--;
        }
        currentItem = null;

        if(currentElement != null)
        {
            currentElement = null;
        }
    }

    public void UpdateItemCount(int change)
    {
        currentItemCount += change;
        if (currentItemCount >= itemLimit)
        {
            isFull = true;
            currentItemCount = itemLimit; 
        }
        else
        {
            isFull = false;
        }
    }
}
