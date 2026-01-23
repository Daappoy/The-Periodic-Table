using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] enemyColumns;
    public Transform[] warningSlots;
    public Transform[] gameOverSlots;
    public GameObject enemyPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.5f;
    void Start()
    {
        CacheSpecialSlotsOnce();
        SpawnEnemies();
    }

    private void CacheSpecialSlotsOnce()
    {
        if (enemyColumns == null || enemyColumns.Length == 0)
        {
            warningSlots = System.Array.Empty<Transform>();
            gameOverSlots = System.Array.Empty<Transform>();
            return;
        }
        
        warningSlots = new Transform[enemyColumns.Length];
        gameOverSlots = new Transform[enemyColumns.Length];
        
        for (int c = 0; c < enemyColumns.Length; c++)
        {
            Transform column = enemyColumns[c];
            if (column == null)
                continue;

            int count = column.childCount;
            if (count < 2)
            {
                // Not enough slots to have both warning \& gameover meaningfully
                warningSlots[c] = null;
                gameOverSlots[c] = count >= 1 ? column.GetChild(count - 1) : null;
                continue;
            }

            warningSlots[c] = column.GetChild(count - 2);
            gameOverSlots[c] = column.GetChild(count - 1);
        }
    }

    [ContextMenu("Spawn Step")]
    public void SpawnEnemies()
    {
        foreach (Transform column in enemyColumns)
        {
            int slotCount = column.childCount;
            Transform targetSlot = null;

            for (int i = 0; i < slotCount; i++)
            {
                Transform slot = column.GetChild(i);
                if (IsSlotEmpty(slot))
                {
                    targetSlot = slot;
                    break;
                }
            }

            if (targetSlot == null)
                continue;

            if (Random.value <= spawnChance)
            {
                SpawnEnemy(targetSlot);
            }
        }
    }

    bool IsSlotEmpty(Transform slot) => slot != null && slot.childCount == 0;

    void SpawnEnemy(Transform slot)
    {
        GameObject enemyObj = Instantiate(enemyPrefab);
        enemyObj.transform.position = slot.position;
        enemyObj.transform.SetParent(slot, true);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        SetAttributes(enemy);
    }

    public void SetAttributes(Enemy enemy)
    {
        enemy.golongan = Random.Range(1, gameManager.Instance.currentLevel + 1);

        if (enemy.golongan == 2)
        {
            enemy.metalic = true;
            return;
        }

        enemy.metalic = Random.Range(0, 2) == 0;
    }
}
