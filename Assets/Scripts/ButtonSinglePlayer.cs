using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSinglePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartSinglePlayer()
    {
        SceneManager.LoadScene("DifficultySelection", LoadSceneMode.Single);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
    }
}

