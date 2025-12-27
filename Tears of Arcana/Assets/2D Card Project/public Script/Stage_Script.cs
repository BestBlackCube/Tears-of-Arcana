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
                        blackBox = true;
                        stageStart = false;
                    }
                }
            }
        }
    }
}
