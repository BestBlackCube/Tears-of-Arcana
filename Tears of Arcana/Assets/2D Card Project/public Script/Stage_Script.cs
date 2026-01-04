using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Script : MonoBehaviour
{
    public bool stageStart = false;
    public bool blackBox = false;
    public float stage_timer = 0f;
    public float stagePanel_timer = 0f;
    float delay_timer = 0f;
    public Image stageImage;
    public TextMeshProUGUI Stage_Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stageStart)
        {
            if(stage_timer < 2f)
            {
                TextChange();
                stage_timer += Time.deltaTime;
                Color stageColor = stageImage.color;
                stageColor.a = Mathf.Lerp(0.0f, 1.0f, stage_timer / 2f);
                stageImage.color = stageColor;
                Stage_Text.color = stageColor;
            }
            else
            {
                if (delay_timer < 1f) delay_timer += Time.deltaTime;
                else
                {
                    if (stagePanel_timer < 2f)
                    {
                        stagePanel_timer += Time.deltaTime;
                        Color stageColor = stageImage.color;
                        stageColor.a = Mathf.Lerp(1.0f, 0.0f, stagePanel_timer / 2f);
                        stageImage.color = stageColor;
                        Stage_Text.color = stageColor;
                    }
                    else
                    {
                        stage_timer = 0;
                        stagePanel_timer = 0;
                        delay_timer = 0;
                        blackBox = true;
                    }
                }
            }
        }
    }
    void TextChange()
    {
        int Count = PlayerPrefs.GetInt("Stage");
        switch (Count)
        {
            case 1:
                Stage_Text.text = "스테이지 1";
                break;
            case 2:
                Stage_Text.text = "스테이지 2";
                break;
            case 3:
                Stage_Text.text = "스테이지 3";
                break;
            case 4:
                Stage_Text.text = "스테이지 4";
                break;
            case 5:
                Stage_Text.text = "스테이지 5";
                break;



            default:
                break;
        }
    }
}
