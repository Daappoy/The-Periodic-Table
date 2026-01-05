using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Animator SceneTransisionAnimator;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TransisionToScene(int transisionTime,string sceneName)
    {
        Debug.Log("Transision Started");
        SceneTransisionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transisionTime + 0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
        SceneTransisionAnimator.SetTrigger("End");
    }
}
