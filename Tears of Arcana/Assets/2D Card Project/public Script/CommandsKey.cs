using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandsKey : MonoBehaviour
{
    [SerializeField] GameObject TurnButton;
    [SerializeField] GameObject StatusOption;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            if(Input.GetKeyDown(KeyCode.T)) // 턴 종료 버튼 생성
            {
                TurnButton.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.R)) // 재시삭 버튼 생성
            {
                SceneManager.LoadScene("battleScene");
            }
            if(Input.GetKeyDown(KeyCode.Q)) // 재시삭 버튼 생성
            {
                if (!StatusOption.activeSelf) StatusOption.SetActive(true);
                else StatusOption.SetActive(false);
            }
            
            if(Input.GetKeyDown(KeyCode.S)) // 현재 스테이지 값 로그값
            {
                int Count = PlayerPrefs.GetInt("Stage");
                Debug.Log("Stage Count : " + Count);
            }
            if(Input.GetKeyDown(KeyCode.Escape)) // 강제 종료
            {
                Application.Quit();
            }
        }
    }
}
