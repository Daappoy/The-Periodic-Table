using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        InGame,
        Paused,
        GameOver,
    }

    public enum SpecialSlotState
    {
        Normal,
        Warning
    }
    
    public TextMeshProUGUI scoreText;
    public GameState currentState;
    public SpecialSlotState specialSlotState;
    public EnemySpawner enemySpawner;
    public ElementSlotManager elementSlotManager;
    public static GameManager Instance;
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
        ScoreManager.Instance.totalScore = score;
        ScoreUpdate();
        //ngecek kalo dah bisa level up
        if (enemiesDefeated == 20)
        {
            LevelUp();
        }
    }

    public void ResetGame()
    {
        currentLevel = 2;
        enemiesDefeated = 0;
        score = 0;
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

    public void ScoreUpdate()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.totalScore.ToString();
    }
}
