using Mono.Cecil.Cil;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSlot : ItemSlot
{
    // SlotType slotType = SlotType.Element;
    public int slotLevel;
    [SerializeField] public bool isTampered = false;


    public GameObject spawnItem(GameObject itemPrefab)
    {
        if (!isFull && slotLevel <= gameManager.Instance.currentLevel)
        {
            //spawn dulu itemnya
            GameObject itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            //set reference ke slot ini
            currentItem = itemInstance.GetComponent<DragDrop>();
            currentItemCount++;
            currentItem.itemSlotscript = this;

            //set parent ke slot ini
            itemInstance.transform.SetParent(transform);
            itemInstance.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            itemInstance.GetComponent<DragDrop>().lastParent = GetComponent<RectTransform>();
            itemInstance.GetComponent<RectTransform>().localScale = Vector3.one;
            isFull = true;
            
            return itemInstance; // Return GameObject yang baru di-spawn
        }
        else if (isFull)
        {
            Debug.Log("Slot is already filled");
        }
        else if (slotLevel > gameManager.Instance.currentLevel)
        {
            Debug.Log("Level too low to spawn this slot of elements");
        }
        
        return null; // Return null jika tidak berhasil spawn
    }
}
