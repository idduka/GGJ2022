using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

    

public class ButtonNewScene : MonoBehaviour
{
    // Start is called before the first frame update

    public void NextScene()
    {
        SceneManager.LoadScene("GameMain");
    }
}

