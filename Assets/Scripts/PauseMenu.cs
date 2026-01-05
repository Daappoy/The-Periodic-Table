using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameManager.GameState currentState;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button[] BackToMainMenuButton;
    public Button RetryButton;

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
        if (GameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            GameIsPaused = false;
        }
        else if (!GameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            GameIsPaused = true;
        }
    }

    public void BackToMainMenu()
    {
        foreach (Button button in BackToMainMenuButton)
        {
            button.interactable = false;
        }
        Time.timeScale = 1f;
        ScoreManager.Instance.SaveScore();
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(1, "MainMenu"));
        Debug.Log("Loading Main Menu");
        GameIsPaused = false;
    }

    public void Retry()
    {
        ScoreManager.Instance.SaveScore();
        RetryButton.interactable = false;
        Time.timeScale = 1f;
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(2, "GameScene"));
        Debug.Log("Retrying Game");
        GameIsPaused = false;
    }
}
