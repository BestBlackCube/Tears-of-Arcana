using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setting_Script : MonoBehaviour
{
    public bool Setting_Click = false;
    bool Setting = false;
    void OnMouseDown()
    {
        if (Setting) Setting_Click = true;
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
