using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] enemyColumns;
    public GameObject enemyPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.5f;

    void Start()
    {
        SpawnEnemies();
    }

    [ContextMenu("Spawn Step")]
    public void SpawnEnemies()
    {
        foreach (Transform column in enemyColumns)
        {
            // Ambil semua Slot (child transform)
            int slotCount = column.childCount;
            Transform[] slots = new Transform[slotCount];

            for (int i = 0; i < slotCount; i++)
                slots[i] = column.GetChild(i);

            // Cari SLOT PERTAMA yang kosong (childCount == 0)
            Transform targetSlot = null;

            for (int i = 0; i < slots.Length; i++)
            {
                if (IsSlotEmpty(slots[i]))
                {
                    targetSlot = slots[i];
                    break;
                }
            }

            // Jika semua slot terisi, lanjut ke column berikutnya
            if (targetSlot == null)
                continue;

            // Roll probability
            if (Random.value <= spawnChance)
            {
                SpawnEnemy(targetSlot);
            }
            // Jika gagal, slot dibiarkan kosong (tidak spawn apa pun)
        }
    }

    bool IsSlotEmpty(Transform slot)
    {
        // Slot kosong berarti tidak punya child
        return slot.childCount == 0;
    }

    void SpawnEnemy(Transform slot)
    {
        GameObject enemyObj = Instantiate(enemyPrefab);

        // Posisi diambil dari slot
        enemyObj.transform.position = slot.position;

        // Parent di set ke slot → slot dianggap terisi
        enemyObj.transform.SetParent(slot, true);

        // Set atribut seperti biasa
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        SetAttributes(enemy);
    }

    public void SetAttributes(Enemy enemy)
    {
        enemy.golongan = Random.Range(1, GameManager.instance.currentLevel + 1);

        if (enemy.golongan == 2)
        {
            enemy.metalic = true;
            return;
        }
        enemy.metalic = Random.Range(0, 2) == 0;
    }
}
