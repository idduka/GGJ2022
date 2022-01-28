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
    public Camera player1camera;
   public Camera player2camera;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Exit"))
        {
            if (_menuShown)
            {
                // Remove menu and resume game.
                player1camera.enabled = true;
                player2camera.enabled = true;

                SceneManager.UnloadSceneAsync("MainMenu");
                
                Time.timeScale = 1.0f;

            }
            else
            {
                
                // Pause game and show menu.
                Time.timeScale = 0.0f;


               player2camera.enabled = false;
                             
            
               SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

               
            }
            _menuShown = !_menuShown;
        }
    }

    }
