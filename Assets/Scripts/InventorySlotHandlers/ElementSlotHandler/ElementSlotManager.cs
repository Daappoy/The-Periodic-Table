using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class SlotGroupElements
{
    //tiap group masing-masing punya 3 slot
    public ElementSlot[] slots;
    public GameObject refreshButton;
    public bool isRefreshed = false;
}

[System.Serializable]
public class GolonganElemen
{
    public List<Elemen> elemenTiapGolongan;
}

public class ElementSlotManager : MonoBehaviour
{
    public GameObject elementPrefab;

    // ini grup
    public SlotGroupElements[] slotGroups;
    public GolonganElemen[] golonganElements;

    // public List<Elemen> spawnedElements = new List<Elemen>();

    public List<Elemen> semuaElements = new List<Elemen>();

    [ContextMenu("Spawn Element In Slot")]
    public void SpawnElementInSlot()
    {
        for (int i = 0; i < slotGroups.Length; i++)
        {
            SlotGroupElements group = slotGroups[i];
            group.isRefreshed = false;

            if (i < golonganElements.Length)
            {
                //list yang ga keliatan di inspector tapi basically nyimpen semua element yang available
                List<Elemen> availableElements = AvailableElements(semuaElements);
                //tiap slot di group itu (3 slot per row atau group)
                for (int j = 0; j < group.slots.Length; j++)
                {
                    ElementSlot slot = group.slots[j];
                    slot.isTampered = false;
                    //make sure ini yang available tetep ada isinya
                    if (availableElements.Count > 0)
                    {
                        Elemen randomElement = availableElements[Random.Range(0, availableElements.Count)];
                        SpawnElementWithData(slot, randomElement);
                    }
                }
            }
        }
    }
    [ContextMenu("Clear All Slots")]
    public void ClearAllSlots()
    {
        for (int i = 0; i < slotGroups.Length; i++)
        {
            SlotGroupElements group = slotGroups[i];
            for (int j = 0; j < group.slots.Length; j++)
            {
                ElementSlot slot = group.slots[j];
                if (slot.isFull && slot.currentItem != null)
                {
                    Destroy(slot.currentItem.gameObject);
                    slot.currentItem = null;
                    slot.currentItemCount = 0;
                    slot.isFull = false;
                }
            }
        }
    }
    [ContextMenu("Get Available Elements")]
    public List<Elemen> AvailableElements(List<Elemen> Elements)
    {
        List<Elemen> availableElements = new List<Elemen>();
        foreach (Elemen element in Elements)
        {
            //ini dimana dia ngecek kalo levelnya sesuai apa enggak untuk dimasukin ke slot
            if (element.elementLevel <= GameManager.instance.currentLevel)
            {
                availableElements.Add(element);
            }
        }
        return availableElements;
    }

    //ini function buat masukkin data ke item yang di spawn
    public void SpawnElementWithData(ElementSlot slot, Elemen elementData)
    {
        if (elementData.elementLevel <= GameManager.instance.currentLevel && slot.slotLevel <= GameManager.instance.currentLevel)
        {
            GameObject spawnedItem = slot.spawnItem(elementPrefab);

            // Debug check untuk spawnedItem
            if (spawnedItem == null)
            {
                Debug.LogError("spawnedItem is null! Check spawnItem method.");
                return;
            }

            // Coba get component, jika tidak ada maka tambahkan
            displayElement display = spawnedItem.GetComponent<displayElement>();
            if (display == null)
            {
                display = spawnedItem.AddComponent<displayElement>();
                Debug.Log("Added displayElement component to spawned item.");
            }

            //tambahin ke list yang udah di spawn ke SpawnedElements
            // spawnedElements.Add(elementData);
            //make sure si elementnya bisa nunjukkin data yang nanti bakal gw tambahin
            display.InitializeElement(elementData);
        }
    }
    //make sure row elemen slot ini bisa di refresh ke elemen baru atau tidak (return bool)
    public void checkRowRefreshable(int groupIndex)
    {
        if (groupIndex < slotGroups.Length)
        {
            SlotGroupElements group = slotGroups[groupIndex];
            for (int j = 0; j < group.slots.Length; j++)
            {
                ElementSlot slot = group.slots[j];
                //jika salah satu ada yang di tamper, berarti row ini ga bisa di refresh
                if (slot.isTampered)
                {
                    Debug.Log("Row " + groupIndex + " is not refreshable.");
                    return;
                }
            }
        }
        slotGroups[groupIndex].isRefreshed = true;
        Debug.Log("Row " + groupIndex + " is refreshable.");
        refreshRowElement(groupIndex);
    }

    //jika row tersebut bisa di refresh, maka akan di refresh sesuai group golongan yang dipilih
    public void refreshRowElement(int groupIndex)
    {
        if (groupIndex < slotGroups.Length && groupIndex < golonganElements.Length)
        {
            SlotGroupElements group = slotGroups[groupIndex];
            List<Elemen> availableGolElements = AvailableElements(golonganElements[groupIndex].elemenTiapGolongan);
            for (int j = 0; j < group.slots.Length; j++)
            {

                ElementSlot slot = group.slots[j];
                //make sure ini yang available tetep ada isinya
                if (availableGolElements.Count > 0)
                {
                    //hapus dulu element yang lama
                    if (slot.isFull && slot.currentItem != null)
                    {
                        Destroy(slot.currentItem.gameObject);
                        slot.currentItem = null;
                        slot.currentItemCount = 0;
                        slot.isFull = false;
                    }
                    Elemen randomElement = availableGolElements[Random.Range(0, availableGolElements.Count)];
                    SpawnElementWithData(slot, randomElement);
                }
            }
        }
    }

    public void Start()
    {
        ClearAllSlots();
        CollectAllElements();
        SpawnElementInSlot();
        ElementSlotIndicator();
    }
    //ambil semua element yang ada di golonganElements
    public void CollectAllElements()
    {
        for (int i = 0; i < golonganElements.Length; i++)
        {
            foreach (Elemen element in golonganElements[i].elemenTiapGolongan)
            {
                if (!semuaElements.Contains(element))
                {
                    semuaElements.Add(element);
                }
            }
        }
    }

    public void ElementSlotIndicator()
    {
        for (int i = 0; i < slotGroups.Length; i++)
        {
            if (i < GameManager.instance.currentLevel)
            {
                SlotGroupElements group = slotGroups[i];
                for (int j = 0; j < group.slots.Length; j++)
                {
                    ElementSlot slot = group.slots[j];
                    slot.GetComponent<CanvasGroup>().alpha = 1f;
                    group.refreshButton.GetComponent<Button>().interactable = true;
                }
            }
            else 
            {
                SlotGroupElements group = slotGroups[i];
                for (int j = 0; j < group.slots.Length; j++)
                {
                    ElementSlot slot = group.slots[j];
                    slot.GetComponent<CanvasGroup>().alpha = 0.5f;
                    group.refreshButton.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void AddElementToNewSlot(int index)
    {
        if (index < slotGroups.Length)
        {
            SlotGroupElements group = slotGroups[index];
            List<Elemen> availableElements = AvailableElements(semuaElements);
            for (int j = 0; j < group.slots.Length; j++)
            {
                ElementSlot slot = group.slots[j];
                slot.isTampered = false;
                //make sure ini yang available tetep ada isinya
                if (availableElements.Count > 0)
                {
                    Elemen randomElement = availableElements[Random.Range(0, availableElements.Count)];
                    SpawnElementWithData(slot, randomElement);
                }
            }
        }
    }
}
