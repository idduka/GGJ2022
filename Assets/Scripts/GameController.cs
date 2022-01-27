using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool _menuShown = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Exit"))
        {
            if (_menuShown)
            {
                // Remove menu and resume game.
                SceneManager.UnloadSceneAsync("MainMenu");
                Time.timeScale = 1.0f;
            }
            else
            {
                // Pause game and show menu.
                Time.timeScale = 0.0f;
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            }
            _menuShown = !_menuShown;
        }
    }
}
