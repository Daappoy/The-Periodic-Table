using UnityEngine;


public class gameManager : MonoBehaviour
{
    public enum GameState
    {
        InGame,
        Paused,
        GameOver,
    }
    public GameState currentState;
    public ElementSlotManager elementSlotManager;
    public static gameManager Instance;
    public int currentLevel = 2;
    public int enemiesDefeated = 0;
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate GameManager instance found and destroyed.");
        }
    }

    public void EnemyDefeated()
    {
        //what happens when an enemy is destroyed
        enemiesDefeated++;
        score += 10;
        ScoreManager.Instance.TotalScore = score;
        //ngececk kalo dah bisa level up
        if (enemiesDefeated == 3)
        {
            LevelUp();
        }
    }

    public void ResetGame()
    {
        currentLevel = 2;
        enemiesDefeated = 0;
    }
    [ContextMenu("Manual Level Up")]
    public void LevelUp()
    {
        currentLevel++;
        enemiesDefeated = 0;
        Debug.Log("Level Up! Current Level: " + currentLevel);
        elementSlotManager.ElementSlotIndicator();
        elementSlotManager.AddElementToNewSlot(currentLevel - 1);
    }
}
