using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBack : MonoBehaviour
{
    // Start is called before the first frame update

    public void GoBack()
    {

        Scene s = SceneManager.GetSceneByName("MainMenu");
        GameObject[] gameObjects = s.GetRootGameObjects();

        foreach (GameObject go in gameObjects)
        {
            if (go.name == "MenusController")
            {
                SubMenusController Script;
                Script = go.GetComponentInChildren<SubMenusController>();
                Script.closesubmenu = true;   
            }

        }

    }
    
}
