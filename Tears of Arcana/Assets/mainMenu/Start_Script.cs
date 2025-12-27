using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Script : MonoBehaviour
{
    bool Start = false;
    void OnMouseDown()
    {
        if (Start) SceneManager.LoadScene("battleScene");
    }
    void OnMouseOver()
    {
        Start = true;   
    }
    void OnMouseExit()
    {
        Start = false;
    }
}
