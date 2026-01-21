using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Animator sceneTransitionAnimator;

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

    public IEnumerator TransitionToScene(int transitionTime,string sceneName)
    {
        Debug.Log("Transition Started");
        sceneTransitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime + 0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
        sceneTransitionAnimator.SetTrigger("End");
        Debug.Log("Transition Ended");
    }
}
