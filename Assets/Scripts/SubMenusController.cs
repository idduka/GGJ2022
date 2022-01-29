using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenusController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool  closesubmenu = false;
    public bool  opensubmenu = false;
    public string SubMenuName;
    

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (closesubmenu == true)
        {
            
               closesubmenu = false;
                //SceneManager.UnloadSceneAsync("MainMenu");
                //Time.timeScale = 1.0f;
            if (SubMenuName == "story")
            {
                
                SceneManager.UnloadSceneAsync("Story");
                Time.timeScale = 1.0f;
            }

            if (SubMenuName == "help")
            {
                
                SceneManager.UnloadSceneAsync("Help");
                Time.timeScale = 1.0f;


            }

        }

        if (opensubmenu == true)
        {
            if (SubMenuName == "story")
            {
                opensubmenu= false;
                Time.timeScale = 0.0f;
                SceneManager.LoadScene("Story", LoadSceneMode.Additive);
            }

            if (SubMenuName == "help")
            {
                opensubmenu = false;
                Time.timeScale = 0.0f;
                SceneManager.LoadScene("Help", LoadSceneMode.Additive);

            }




        }



    }
  
}
