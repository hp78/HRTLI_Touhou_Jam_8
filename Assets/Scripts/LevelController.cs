using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public string levelToLoad;
    public string currentLevel;

    public Animator levelTransitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().ToString();
    }

    //
    public void StartLevel()
    {
        Time.timeScale = 1f;
    }

    //
    public void RestartLevel()
    {
        levelToLoad = currentLevel;
        GoToNextLevel();
    }

    //
    public void GoToNextLevel(string nextLevelName)
    {
        levelToLoad = nextLevelName;
        GoToNextLevel();
    }

    //
    public void GoToNextLevel()
    {
        Time.timeScale = 0f;
        levelTransitionAnimator.Play("TransitOut");
    }

    //
    public void ChangeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
