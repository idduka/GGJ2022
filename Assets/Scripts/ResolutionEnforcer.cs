using UnityEngine;

public class ResolutionEnforcer : MonoBehaviour
{
    void Start()
    {
        SetRatio(2, 1);
    }
    void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }
}