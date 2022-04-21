using UnityEngine.SceneManagement;
using UnityEngine;

public class GraceController : MonoBehaviour
{
    public string levelToLoad;
    float currHeld = 0.0f;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    void ChangeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        if(currHeld > 2.0f)
        {
            ChangeLevel();
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetButton("Grace"))
        {
            currHeld += Time.deltaTime;
        }
        else
        {
            currHeld = 0;
        }
    }
}
