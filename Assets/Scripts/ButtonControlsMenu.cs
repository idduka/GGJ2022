using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControlsMenu : MonoBehaviour
{
    public SubMenusController MenuController;
public void DisplayStory()

    {
        MenuController.SubMenuName = "story";
        MenuController.opensubmenu = true;

    }

    public void DisplayHelp()
    {
        MenuController.SubMenuName = "help";
        MenuController.opensubmenu = true;
    }
    public void DisplaySinglePlayer()
    {
        MenuController.SubMenuName = "singleplayer";
        MenuController.opensubmenu = true;
    }

}
