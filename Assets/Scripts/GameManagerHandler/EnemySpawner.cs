using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] enemyColumns;
    public Transform[] spawnSlots;
    public Transform[] warningSlots;
    public Transform[] gameOverSlots;
    public GameObject enemyPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.5f;
    void Start()
    {
        CacheSpecialSlotsOnce();
        InitiateSpawnEnemies();
    }

    private void CacheSpecialSlotsOnce()
    {
        if (enemyColumns == null || enemyColumns.Length == 0)
        {
            spawnSlots = System.Array.Empty<Transform>();
            warningSlots = System.Array.Empty<Transform>();
            gameOverSlots = System.Array.Empty<Transform>();
            return;
        }
        
        warningSlots = new Transform[enemyColumns.Length];
        gameOverSlots = new Transform[enemyColumns.Length];
        spawnSlots = new Transform[enemyColumns.Length];
        
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
            spawnSlots[c] = column.GetChild(0);
        }
    }

    [ContextMenu("Spawn Step")]
    public void InitiateSpawnEnemies()
    {
        if (enemyColumns == null)
            return;

        foreach (Transform column in enemyColumns)
        {
            if (column == null)
                continue;

            int slotCount = column.childCount;
            if (slotCount == 0)
                continue;

            // decide first whether to spawn for this column
            if (Random.value > spawnChance)
                continue;

            // shift enemies down to free slot 0 (one step), starting from bottom
            for (int i = slotCount - 1; i >= 1; i--)
            {
                Transform slot = column.GetChild(i);
                Transform above = column.GetChild(i - 1);

                if (IsSlotEmpty(slot) && !IsSlotEmpty(above) && above.childCount > 0)
                {
                    Transform child = above.GetChild(0);
                    child.SetParent(slot, true);
                    child.position = slot.position;
                }
            }

            // spawn at slot 0 if now empty
            Transform targetSlot = column.GetChild(0);
            if (IsSlotEmpty(targetSlot))
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
        enemy.golongan = Random.Range(1, GameManager.Instance.currentLevel + 1);

        if (enemy.golongan == 2)
        {
            enemy.metalic = true;
            return;
        }

        enemy.metalic = Random.Range(0, 2) == 0;
    }
    
    //check if there is any enemy in the special slots
    //check on warning slot
    public bool IsEnemyInWarningSlot()
    {
        foreach (Transform slot in warningSlots)
        {
            if (slot != null && slot.childCount > 0)
            {
                return true;
            }
        }
        return false;
    }
    //check on game over slot
    public bool IsEnemyInGameOverSlot()
    {
        foreach (Transform slot in gameOverSlots)
        {
            if (slot != null && slot.childCount > 0)
            {
                return true;
            }
        }
        return false;
    }
}
