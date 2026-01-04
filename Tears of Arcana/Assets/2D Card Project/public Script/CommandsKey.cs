using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsKey : MonoBehaviour
{
    [SerializeField] GameObject TurnButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                TurnButton.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                int Count = PlayerPrefs.GetInt("Stage");
                Debug.Log("Stage Count : " + Count);
            }
        }
    }
}
