using UnityEngine;
using UnityEngine.SceneManagement;

public class SetDifficulty : MonoBehaviour
{
    string difficultySelected = "hard";

    public void SelectEasy()
    {
        difficultySelected = "easy";
        LoadGameMain();
    }

    public void SelectMedium()
    {
        difficultySelected = "medium";
        LoadGameMain();
    }

    public void SelectHard()
    {
        difficultySelected = "hard";
        LoadGameMain();
    }

    public void LoadGameMain()
    {
        SceneManager.LoadScene("GameMain", LoadSceneMode.Single);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SetDifficultyLevel(difficultySelected);
    }

    private void SetDifficultyLevel(string difficulty)
    {
        Scene s = SceneManager.GetSceneByName("GameMain");
        GameObject[] gameObjects = s.GetRootGameObjects();

        foreach (GameObject go in gameObjects)
        {
            if (go.name == "Player1Zone")
            {
                DefenderAI AIscript;
                AIscript = go.GetComponentInChildren<DefenderAI>();
                AIscript.enabled = false;
                AIscript.SetDifficulty(difficulty);
            }
            if (go.name == "Player2Zone")
            {
                DefenderAI AIscript;
                AIscript = go.GetComponentInChildren<DefenderAI>();
                AIscript.enabled = true;
                AIscript.SetDifficulty(difficulty);
            }
        }
    }
}
