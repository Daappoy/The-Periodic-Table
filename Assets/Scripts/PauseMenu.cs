using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameManager.GameState currentState;
    private static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button[] backToMainMenuButton;
    public Button retryButton;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (gameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            gameIsPaused = false;
        }
        else if (!gameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            gameIsPaused = true;
        }
    }

    public void BackToMainMenu()
    {
        foreach (Button button in backToMainMenuButton)
        {
            button.interactable = false;
        }
        Time.timeScale = 1f;
        ScoreManager.Instance.SaveScore();
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransitionToScene(1, "MainMenu"));
        Debug.Log("Loading Main Menu");
        gameIsPaused = false;
    }

    public void Retry()
    {
        ScoreManager.Instance.SaveScore();
        retryButton.interactable = false;
        Time.timeScale = 1f;
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransitionToScene(2, "GameScene"));
        Debug.Log("Retrying Game");
        gameIsPaused = false;
    }
}
