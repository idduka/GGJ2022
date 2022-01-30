using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool _menuShown = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    public Camera player1camera;
    public Camera player2camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Exit"))
        {
            if (_menuShown)
            {
                // Remove menu and resume game.
                //player1camera.enabled = true;
                //player2camera.enabled = true;

                SceneManager.UnloadSceneAsync("MainMenu");

                Time.timeScale = 1.0f;
            }
            else
            {
                // Pause game and show menu.
                Time.timeScale = 0.0f;

                SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            }
            _menuShown = !_menuShown;
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (player1camera != null)
        {
            player1camera.enabled = false;
            player2camera.enabled = false;
            player1camera.enabled = true;
            player2camera.enabled = true;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
