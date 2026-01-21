using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OldEnemySpawner : MonoBehaviour
{
    public GameObject[] enemyColumns;
    public GameObject enemyPrefab;
    public GameObject EmptySlotPrefab;
    public int enemiesPerColumn = 1; 
    [Range(0f, 1f)]
    public float spawnChance = 0.5f; 

    void Start()
    {
        SpawnEnemies();
    }
    [ContextMenu("Manual spawn New Enemies")]
    public void SpawnEnemies()
    {
        Debug.Log("Spawning enemies...");
        foreach (GameObject column in enemyColumns)
        {
            // Spawn sejumlah enemiesPerColumn dengan 50% chance
            for (int i = 0; i < enemiesPerColumn; i++)
            {
                // 50% chance untuk spawn enemy
                if (Random.Range(0f, 1f) <= spawnChance)
                {
                    //spawn enemy
                    GameObject enemyInstance = Instantiate(enemyPrefab);
                    enemyInstance.transform.SetParent(column.transform, false); // false = gunakan local position
                    enemyInstance.transform.SetAsFirstSibling();

                    SetAttributes(enemyInstance.GetComponent<Enemy>());
                }
                else
                {
                    // Jika tidak spawn, buat GameObject kosong sebagai placeholder
                    GameObject emptySlot = Instantiate(EmptySlotPrefab);
                    emptySlot.transform.SetParent(column.transform, false); // false = gunakan local position
                    emptySlot.transform.SetAsFirstSibling();
                }
            }
        }
    }
    public void SetAttributes(Enemy enemy)
    {
        enemy.golongan = Random.Range(1, gameManager.Instance.currentLevel + 1);
        if  (enemy.golongan == 2)
        {
            enemy.metalic = true;
            return;
        }
        enemy.metalic = Random.Range(0, 2) == 0;
    }

}
