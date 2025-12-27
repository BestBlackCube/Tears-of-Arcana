using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen_Script : MonoBehaviour
{
    public CardDeck_Script deck;
    public Stage_Script stage;
    public EnemyObjectSet_Script enemySet;
    public GameObject loading;
    public Image blackScreen;
    public float Loading_timer = 0f;
    public float Screen_timer = 0f;
    bool screen_A = false;
    float colorA = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        enemySet.NextStage = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Loading_timer < 5f) Loading_timer += Time.deltaTime;
        else
        {
            loading.SetActive(!enabled);
            screen_A = true;
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
            }
        }
        if(stage.blackBox)
        {
            this.gameObject.SetActive(!enabled);
            stage.blackBox = false;
        }
    }
}
