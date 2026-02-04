using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotGroupWeapon
{
    public WeaponSlot[] slots;
    public Transform spawnPos;
}

public class WeaponSlotManager : MonoBehaviour
{
    public SlotGroupWeapon[] weaponSlotGroups;
    public GameObject bulletPrefab;
    public float shootDelay = 0.5f;
    public ElementSlotManager elementSlotManager;
    public EnemySpawner enemySpawner;

    public void Shoot()
    {
        StartCoroutine(ShootWithDelay());
    }

    private IEnumerator ShootWithDelay()
    {
        elementSlotManager.ClearAllSlots();
        elementSlotManager.SpawnElementInSlot();


        for (int i = 0; i < weaponSlotGroups.Length; i++)
        {
            SlotGroupWeapon group = weaponSlotGroups[i];

            // Tembak
            for (int j = 0; j < group.slots.Length; j++)
            {
                WeaponSlot slot = group.slots[j];
                if (slot.isFull && slot.currentItem != null && slot.currentElement != null)
                {
                    // Spawn bullet
                    GameObject bulletInstance = Instantiate(bulletPrefab, group.spawnPos.position, group.spawnPos.rotation);

                    // Assign data elemen ke bullet
                    Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
                    if (bulletScript != null)
                    {
                        bulletScript.element = slot.currentElement;
                        Debug.Log("Bullet fired with element: " + slot.currentElement.elementName);
                        Debug.Log("Element Golongan: " + slot.currentElement.Golongan + ", Metalic: " + slot.currentElement.Metalic);
                    }

                    // Hapus item dari slot abis ditembak
                    if (slot.currentItem != null)
                    {
                        Destroy(slot.currentItem.gameObject);
                    }
                    slot.RemoveItem();

                    // jeda
                    if (j < group.slots.Length - 1)
                    {
                        yield return new WaitForSeconds(shootDelay);
                    }
                }
            }
        }
        yield return new WaitForSeconds(2f);
        enemySpawner.InitiateSpawnEnemies();
    }
}
