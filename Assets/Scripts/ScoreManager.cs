using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int totalScore = 0;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            totalScore = PlayerPrefs.GetInt("PlayerScore");
        }
        else
        {
            totalScore = 0;
        }
    }
    //note: ini bakal di save sebagai json later
    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", totalScore);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
        }
        PlayerPrefs.Save();
    }
}