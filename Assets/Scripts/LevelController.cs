using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public string levelToLoad;
    public string currentLevel;

    public Image playerHP;
    float playerHPWidth = 280;

    public Image bossHP;
    float bossHPWidth = 470;

    public Animator levelTransitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentLevel = SceneManager.GetActiveScene().name;
        Time.timeScale = 0f;
    }

    public void UpdatePlayerHealth(float valPercent)
    {
        playerHP.fillAmount = valPercent;
    }

    public void UpdateBossHealth(float valPercent)
    {
        bossHP.fillAmount = valPercent;
    }

    //
    public void StartLevel()
    {
        Time.timeScale = 1f;
    }

    //
    public void PlayerDiedSeq()
    {
        levelTransitionAnimator.Play("PlayerDeath");
    }

    //
    public void RestartLevel()
    {
        levelToLoad = currentLevel;
        SceneManager.LoadScene(levelToLoad);
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
        levelTransitionAnimator.Play("EndLevel");
    }

    //
    public void ChangeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
