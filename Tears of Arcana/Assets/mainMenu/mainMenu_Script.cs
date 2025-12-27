
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu_Script : MonoBehaviour
{
    public GameObject[] menuButton;
    public Start_Script startButton;
    public Setting_Script settingButton;
    //public Exit_Script exitButton;
    
    bool SettingTF = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!SettingTF)
        {
            if (settingButton.Setting_Click)
            {
                SceneManager.LoadScene("SettingScene", LoadSceneMode.Additive);
                PlayerPrefs.SetInt("Setting_panel", 0);
                PlayerPrefs.Save();
                ButtonActive(false);
                SettingTF = true;
            }
        }
        else
        {
            if(!settingButton.Setting_Click)
            {
                ButtonActive(true);
                SettingTF = false;
            }
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("Setting_panel") == 2) settingButton.Setting_Click = false;
    }
    void ButtonActive(bool active)
    {
        for(int i = 0; i < menuButton.Length; i++)
        {
            menuButton[i].SetActive(active);
        }
    }
}
