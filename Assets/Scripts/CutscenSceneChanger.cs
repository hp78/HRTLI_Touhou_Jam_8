using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenSceneChanger : MonoBehaviour
{
    public string levelToLoad;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void ChangeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
