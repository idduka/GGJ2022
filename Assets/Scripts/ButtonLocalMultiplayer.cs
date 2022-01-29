using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class ButtonLocalMultiplayer : MonoBehaviour
{
    // Start is called before the first frame update

    public void StartLocalMultiplayer()
    {
        SceneManager.LoadScene("GameMain", LoadSceneMode.Single);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Scene s = SceneManager.GetSceneByName("GameMain");
        GameObject[] gameObjects = s.GetRootGameObjects();

        foreach (GameObject go in gameObjects)
        {
            Debug.Log(go.name);
            if (go.name == "Player1Zone")
            {
                DefenderAI AIscript;
                AIscript = go.GetComponentInChildren<DefenderAI>();
                AIscript.enabled = false;
            }
            if (go.name == "Player2Zone")
            {
                DefenderAI AIscript;
                AIscript = go.GetComponentInChildren<DefenderAI>();
                AIscript.enabled = false;
            }
        }
    }
}

