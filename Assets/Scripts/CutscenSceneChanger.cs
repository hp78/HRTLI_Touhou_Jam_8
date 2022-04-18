using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenSceneChanger : MonoBehaviour
{
    public string levelToLoad;
    public void ChangeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
