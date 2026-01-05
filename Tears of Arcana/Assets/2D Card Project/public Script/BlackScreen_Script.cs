using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen_Script : MonoBehaviour
{
    public CardDeck_Script deck;
    public Stage_Script stage;
    public EnemyObjectSet_Script ObjectSet;
    public GameObject loading;
    public Image blackScreen;
    [SerializeField] TextMeshProUGUI StroyText;
    public float Loading_timer = 0f;
    public float Screen_timer = 0f;
    float Delay_timer = 0f;
    public bool PlayerNext = false;

    bool screen_A = false;
    bool battleStart = false;
    bool textStart = false;
    [SerializeField] bool nextTextAuto = false;
    float colorA = 2f;
    float text_timer = 0f;
    [SerializeField] float textAuto_timer = 0f;
    string saveText;
    int i = 0;
    int nextText = 0;
    // Start is called before the first frame update
    void Awake()
    {
        ObjectSet.NextStage = true;
        if (StroyText.text != "") StroyText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(nextTextAuto)
        {
            if (textAuto_timer < 10f) textAuto_timer += Time.deltaTime;
            else
            {
                nextText++;
                nextTextAuto = false;
            }
        }
        else textAuto_timer = 0;
        if (textStart)
        {
            if (Input.GetKey(KeyCode.Space)) nextText++;
            if (Input.GetKey(KeyCode.Space) && battleStart) screen_A = true;
        }
        if(PlayerNext)
        {
            if(Screen_timer < colorA)
            {
                Screen_timer += Time.deltaTime;
                Color screen_color = blackScreen.color;
                screen_color.a = Mathf.Lerp(0.0f, 1.0f, Screen_timer);
                blackScreen.color = screen_color;
            }
            else
            {
                if(Delay_timer < 1f) Delay_timer += Time.deltaTime;
                else
                {
                    ObjectSet.NextStageInput();
                    stage.gameObject.SetActive(true);
                    if (StroyText.text != "") StroyText.text = "";
                    loading.SetActive(true);
                    Screen_timer = 0;
                    Delay_timer = 0;
                    PlayerNext = false;
                }
            }
        }
        else
        {
            if (Loading_timer < 5f) Loading_timer += Time.deltaTime;
            else
            {
                TextCount();
                TextChange(saveText, saveText.Length);
                loading.SetActive(false);
                textStart = true;
                if(battleStart)
                {
                    nextText = 0;
                    screen_A = true;
                    battleStart = false;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if(screen_A)
        {
            if (StroyText.text != "") StroyText.text = "";
            if (Screen_timer < colorA)
            {
                Screen_timer += Time.deltaTime;
                Color screen_color = blackScreen.color;
                screen_color.a = Mathf.Lerp(1.0f, 0.0f, Screen_timer / 2f);
                deck.Cardinput = true;
                blackScreen.color = screen_color;
            }
            else
            {
                stage.stageStart = true;
                screen_A = false;
            }
        }
        if(stage.blackBox && !PlayerNext)
        {
            Loading_timer = 0;
            Screen_timer = 0;
            stage.blackBox = false;
            stage.stageStart = false;
            stage.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    void TextChange(string text, int length)
    {
        if (i == length) nextTextAuto = true;
        if(i < length)
        if (text_timer < 0.1f) text_timer += Time.deltaTime;
        else
        {
            StroyText.text += text[i];
            i++;
            text_timer = 0;
        }
    }
    void TextCount()
    {
        int i = PlayerPrefs.GetInt("Stage");
        switch(i)
        {
            case 100:
                if(nextText == 0)
                saveText = "xxxx년 06월 15일\n" +
                    "세계에서 저명한 연구소에서 핵기술\n" +
                    "관련 기밀 프로젝트를 진행하여\n" +
                    "나를 스카우트하기 위해 제의가 들어와\n" +
                    "프로젝트를 진행하였다.";
                if (nextText == 1)
                    saveText = "xxxx년 07월 13일\n" +
                        "기밀 프로젝트를 진행한 지도 벌써 2년이 지났다.\n" +
                        "마무리 단계에 접어들면서, 오랜만에 가족들을 만날\n" +
                        "생각에 잠겨 있었다. 그러나 그때, 내 인생을\n" +
                        "송두리째 바꿀 사건이 일어났다.";
                if (nextText == 2)
                    saveText = "멀리서 연구원처럼 보이는 사람이\n" +
                        "섬뜩한 웃음을 지으며 나와 눈을 마주치는 순간\n" +
                        "엄청난 굉음과 함께 연구소가 폭발했다.\n";
                if(nextText == 3)
                    saveText = "사망자 4명 부상자 1명 실종 1명";
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15: 
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;

            default:
                break;
        }
    }
}
