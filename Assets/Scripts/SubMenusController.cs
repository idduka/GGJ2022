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
       // Time.timeScale = 1.0f;
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
                
            }

            if (SubMenuName == "help")
            {
             
                SceneManager.UnloadSceneAsync("Help");
           
            }

            if (SubMenuName == "singleplayer")
            {

                SceneManager.UnloadSceneAsync("DifficultySelection");

            }
        }

        if (opensubmenu == true)
        {
            if (SubMenuName == "story")
            {
                opensubmenu= false;
               
                SceneManager.LoadScene("Story", LoadSceneMode.Additive);
            }

            if (SubMenuName == "help")
            {
                opensubmenu = false;
      
                SceneManager.LoadScene("Help", LoadSceneMode.Additive);

            }

            if (SubMenuName == "singleplayer")
            {
                opensubmenu = false;

                SceneManager.LoadScene("DifficultySelection", LoadSceneMode.Additive);

            }




        }



    }
  
}
