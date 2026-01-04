using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
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

    public float Loading_timer = 0f;
    public float Screen_timer = 0f;
    float Delay_timer = 0f;
    public bool PlayerNext = false;

    bool screen_A = false;
    float colorA = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        ObjectSet.NextStage = true;
    }

    // Update is called once per frame
    void Update()
    {
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
                loading.SetActive(false);
                screen_A = true;
            }
        }
    }
    void FixedUpdate()
    {
        if(screen_A)
        {
            if(Screen_timer < colorA)
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
}
