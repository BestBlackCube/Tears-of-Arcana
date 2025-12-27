using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting_Panel : MonoBehaviour
{
    bool Setting = false;
    void Start()
    {

    }
    void OnMouseDown()
    {
        if (Setting)
        {
            PlayerPrefs.SetInt("Setting_panel", 2);
            PlayerPrefs.Save();
            SceneManager.UnloadSceneAsync("SettingScene");
        }
    }
    void OnMouseOver()
    {
        Setting = true;
    }
    void OnMouseExit()
    {
        Setting = false;
    }
}
