using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] public ItemSlot itemSlotscript;
    [SerializeField] public RectTransform lastParent;
    public displayElement currentDisplayElement;

    private void Awake()
    {
        currentDisplayElement = GetComponent<displayElement>();
        if (lastParent == null)
        {
            lastParent = GetComponent<RectTransform>().parent as RectTransform;
        }
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        transform.SetParent(canvas.transform);

        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        if (itemSlotscript != null && itemSlotscript is WeaponSlot)
        {
            itemSlotscript.RemoveItem();
        }
        else if (itemSlotscript != null && itemSlotscript is ElementSlot)
        {
            Debug.Log("Element item taken from Element slot");
            ElementSlot elementSlotParent = itemSlotscript as ElementSlot;
            elementSlotParent.isTampered = true;
            elementSlotParent.isFull = false;

            itemSlotscript.RemoveItem();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            // Kembali ke parent sebelumnya jika tidak dimasukkan ke slot
            BackToLastParent();
        }
    }

    public void BackToLastParent()
    {
        // Kembali ke parent sebelumnya jika tidak dimasukkan ke slot
        if (lastParent != null)
        {
            transform.SetParent(lastParent);
            rectTransform.anchoredPosition = lastParent.anchoredPosition;
        }

        //make sure Element slot nya dapet data item seperti sebelumnya
        if (itemSlotscript != null && itemSlotscript is ElementSlot)
        {
            ElementSlot elementSlotParent = itemSlotscript as ElementSlot;
            elementSlotParent.isTampered = false;
            elementSlotParent.isFull = true;
            itemSlotscript.currentItem = this;
            elementSlotParent.UpdateItemCount(1);
        }

        //make sure Weapon slot nya dapet data item seperti sebelumnya
        if (itemSlotscript != null && itemSlotscript is WeaponSlot)
        {
            itemSlotscript.UpdateItemCount(1);
            itemSlotscript.isFull = true;
            itemSlotscript.currentItem = this;
            itemSlotscript.currentElement = currentDisplayElement.element; // pastikan elemen disimpan di slot
        }
    }
}
