using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : ItemSlot, IDropHandler
{
    // SlotType slotType = SlotType.Weapon;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Weapon dropped on slot");
        if (eventData.pointerDrag != null && currentItemCount < itemLimit)
        {
            InsertItem(eventData);
        }
        else
        {
            Debug.Log("Slot is already filled");
            eventData.pointerDrag.GetComponent<DragDrop>().BackToLastParent();
        }
    }

    public void InsertItem(PointerEventData eventData)
    {
        // Set reference ke slot ini
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        dragDrop.lastParent = GetComponent<RectTransform>();

        currentElement = dragDrop.currentDisplayElement.element; // tambahkan element data ke slot

        currentItem = dragDrop;
        currentItemCount++;
        isFull = true;
        currentItem.itemSlotscript = this;

        eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        eventData.pointerDrag.transform.SetParent(transform);
    }
}

