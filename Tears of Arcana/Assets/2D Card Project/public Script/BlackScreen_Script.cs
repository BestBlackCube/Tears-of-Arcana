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
    [SerializeField] TextMeshProUGUI StoryText;
    public float Loading_timer = 0f;
    public float Screen_timer = 0f;
    float Delay_timer = 0f;
    public bool PlayerNext = false;

    bool screen_A = false;
    bool battleStart = false;
    [SerializeField] bool battleTextStart = false;
    [SerializeField] bool textStart = false;
    [SerializeField] bool nextTextAuto = false;
    [SerializeField] bool Text = false;
    float colorA = 2f;
    float text_timer = 0f;
    [SerializeField] float textAuto_timer = 0f;
    string saveText;
    [SerializeField] int i = 0;
    [SerializeField] int nextText = 0;
    [SerializeField] GameObject player; 
    // Start is called before the first frame update
    void Awake()
    {
        ObjectSet.NextStage = true;
        if (StoryText.text != "") StoryText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(nextTextAuto)
        {
            if(player.GetComponent<AudioSource>().clip != null && i >= saveText.Length)
            {
                player.GetComponent<AudioSource>().clip = null;
                player.GetComponent<AudioSource>().loop = false;
                player.GetComponent<AudioSource>().Stop();
            }
            if (textAuto_timer < 10f) textAuto_timer += Time.deltaTime;
            else
            {
                nextText++;
                i = 0;
                StoryText.text = "";
                nextTextAuto = false;
            }
        }
        else textAuto_timer = 0;
        if (textStart)
        {
            TextCount();
            TextChange(saveText, saveText.Length);
            if (Input.GetMouseButtonDown(0) && !nextTextAuto)
            {
                player.GetComponent<AudioSource>().loop = false;
                player.GetComponent<AudioSource>().Stop();
                StoryText.text = saveText;
                i = saveText.Length;
                nextTextAuto = true;
            }
            if (Input.GetMouseButtonDown(0) && textAuto_timer > 1f)
            {
                textAuto_timer = 11f;
            }
            if (Input.GetMouseButtonDown(0) && battleTextStart)
            {
                player.GetComponent<AudioSource>().clip = null;
                player.GetComponent<AudioSource>().loop = false;
                player.GetComponent<AudioSource>().Stop();
                nextText = 0;
                i = 0;
                nextTextAuto = false;
                battleStart = true;
                textStart = false;
            }
        }
        if (PlayerNext)
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
                ObjectSet.audioSource.Stop();
                if (Delay_timer < 1f) Delay_timer += Time.deltaTime;
                else
                {
                    Text = false;
                    ObjectSet.NextStageInput();
                    stage.gameObject.SetActive(true);
                    if (StoryText.text != "") StoryText.text = "";
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
                if (!Text && !battleStart)
                {
                    textStart = true;
                    Text = true;
                }
                loading.SetActive(false);
                if(battleStart)
                {
                    nextText = 0;
                    screen_A = true;
                    battleTextStart = false;
                    battleStart = false;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if(screen_A)
        {
            if (StoryText.text != "") StoryText.text = "";
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
        if(player.GetComponent<AudioSource>().clip == null && i < length)
        {
            player.GetComponent<AudioSource>().clip = player.GetComponent<Audio_Script>().audioClips[0];
            player.GetComponent<AudioSource>().loop = true;
            player.GetComponent<AudioSource>().Play();
        }
        if (i == length) nextTextAuto = true;

        if (i < length)
        {
            if (text_timer < 0.05f) text_timer += Time.deltaTime;
            else
            {
                StoryText.text += text[i];
                i++;
                text_timer = 0;
            }
        }
    }
    void TextCount()
    {
        int i = PlayerPrefs.GetInt("Stage");
        switch(i)
        {
            case 1:
                if (nextText == 0)
                {
                    saveText = "xxx1년 06월 15일\n" + "세계에서 저명한 연구소에서 핵기술\n" + "관련 기밀 프로젝트를 진행하기위해\n" +
                        "나에게 프로젝트 섭외 제의가 들어와\n" + "섭외를 받아드렸다.";
                }
                if (nextText == 1)
                {
                    saveText = "xxx3년 07월 13일\n" + "기밀 프로젝트를 진행한 지도 벌써 2년이 지났다.\n" +
                        "마무리 단계에 접어들면서, 오랜만에 가족들을 만날\n" + "생각에 잠겨 있었다. 그러나 그때, 내 인생을\n" +
                        "송두리째 바꿀 사건이 일어났다.";
                }
                if (nextText == 2)
                {
                    saveText = "멀리서 연구원처럼 보이는 사람이\n" + "섬뜩한 웃음을 지으며 나와 눈을 마주치는 순간\n" +
                        "엄청난 굉음과 함께 연구소가 폭발했다.\n";
                }
                if (nextText == 3)
                {
                    saveText = "사망자 4명 부상자 1명 실종 1명";
                }
                if (nextText == 4)
                {
                    saveText = "xxx6년 06월 5일\n" + "나는 불행 중 다행이라고 해야할까.. 연구소 폭발 당시\n" + 
                        "건물 파면에 깔려 있던 난 구조되었지만 그일로 다리에 큰 장애가 생겼다\n" +
                       "기밀 프로젝트의 책임자였던 난 모든 사건의 책임을 전가되었고,\n" + 
                       "사람들의 비난과 야유를 떠안으며 나의 평범했던 일상은 산산조각이 난다.";
                }
                if (nextText == 5)
                {
                    saveText = "나는 그날의 폭발은 잊혀지지가 않는다..\n" + "매일 밤 악몽이 되어 나를 괴롭히며, 그때의 그 상황이\n" +
                        "나의 마음을 짖누르고 있다.";
                }
                if (nextText == 6)
                {
                    saveText = "xxx6년 06월 6일\n" + "뭔가 이상하다. 매일 나를 괴롭히던 악몽과는 다르게,\n" + 
                        "칠흑 같은 어둠속의 공간 속에 갇혀 있던 꿈이였다..\n" +
                       "나는 당장이라도 이 공간에서 벗어나기 위해 필사적으로 발버둥 쳤다\n" 
                       + "그러다 섬뜩한 기운을 느껴 뒤를 돌아보니, 인간의 형태를 한 무언가가 나에게 말을 건다.";
                }
                if (nextText == 7)
                {
                    saveText = "??? : 찾았다\n" + "나(이현우) : 깜짝이야, 너는 누구야?\n" + 
                        "(나는 깜짝놀라며 그를 응시한다)";
                }
                if (nextText == 8)
                {
                    saveText = "니바스 : 나는 악마 니바스. 너에게 제안을 하기위해 찾아왔지\n" + 
                        "(니바스는 섬뜩한 미소를 지으면 나(이현우)를 응시했다.)";
                }
                if (nextText == 9)
                {
                    saveText = "나(이현우) : 제안이라니 그게 무슨 말이지?\n" + "니바스 : 간단한 게임 하나 하지.";
                }
                if (nextText == 10)
                {
                    saveText = "나(이현우) : 간단한 게임이라니..?\n" + "니바스 : 그래, 네가 이기면 \"과거로 보내줄게\"\n" +
                        "그대신 반대로 진다면 네가 가지고있는 소중한것을 하나 가져가지\n" +
                        "나쁘지 않은 제안이라고 생각한다만?";
                }
                if (nextText == 11)
                {
                    saveText = "고민을 하고 있는 나를 본 니바스는\n" +
                        "나(이현우)에게 가벼운 어투로 말한다\n" +
                        "니바스 : 무엇을 잃게 될지 걱정하는건가? 그건 네 선택이 아닌\n" +
                        "나의 즐거움에 달려있으니까 문제 될건 없지\n" + 
                        "나(이현우) : 그래 안좋을건 없지, 당장 하자.";
                }
                if (nextText == 12)
                {
                    saveText = "그는 그저 악몽을 꾸던 꿈은 일부라 생각해 이 선택이\n"
                        + "자신에게 어떤 미래를 가져올지 알 수 없었다.";
                }
                if (nextText == 13)
                {
                    saveText = "니바스는 기다렸다는 듯 소름 끼치는 웃음을 터뜨리자,\n 붉은 연기가 나(이현우)를 덮쳤다\n" +
                        "니바스 : 그럼 끝에서 만나자고... 인간";
                    if (StoryText.text.Length >= saveText.Length) battleTextStart = true;
                    ObjectSet.audioSource.clip = ObjectSet.audioClip[1];
                    ObjectSet.audioSource.Play();
                }
                break;
            case 2:
                saveText = "";
                ObjectSet.audioSource.clip = ObjectSet.audioClip[3];
                ObjectSet.audioSource.Play();
                battleTextStart = true;
                break;
            case 3:
                saveText = "";
                ObjectSet.audioSource.clip = ObjectSet.audioClip[5];
                ObjectSet.audioSource.Play();
                battleTextStart = true;
                break;
            case 4:
                ObjectSet.audioSource.clip = ObjectSet.audioClip[1];
                ObjectSet.audioSource.Play();
                battleStart = true;
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
