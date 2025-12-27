using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_Script : MonoBehaviour
{
    bool Exit = false;
    void OnMouseDown()
    {
        if (Exit) SceneManager.UnloadSceneAsync("mainMenuScenes");
    }
    void OnMouseOver()
    {
        Exit = true;
    }
    void OnMouseExit()
    {
        Exit = false;
    }
}
