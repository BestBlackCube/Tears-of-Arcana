using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObjectSet_Script : MonoBehaviour
{
    public CardDeck_Script deck;
    public CardDeckField_Script deckField;
    [SerializeField] GameObject turnEnd_prefab;
    public GameObject[] StageCard;
    GameObject card;
    public GameObject[] Monster_Object;
    public GameObject[] Field_inMonster;
    public GameObject TargetArrow_prefab;
    public GameObject[] TargetArrow;
    public GameObject TargetGuide_prefab;
    public GameObject[] TargetGuide;
    public GameObject EnemyStun_prefab;
    public RectTransform[] EnemyStun;
    public GameObject EnemyHpbar_prefab;
    public RectTransform[] EnemyHpbar;
    public GameObject EnemyBossHpbar_prefab;
    public RectTransform EnemyBossHpbar;
    GameObject canvas;
    public GameObject BlackScreen;

    [SerializeField] Sprite[] BackGround_Image;

    public string[] Enemy_Name;
    public bool NextStage = false;
    public bool StageCardInput = true;
    public bool nullAndinput = false;
    bool turn = false;

    public int StageCount = 0;
    public int MonsterCount = 0;
    public int MonsterDeadCount = 0;

    public Vector3[] Field_transform;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GameObject.Find("HPCanvas");
        HpbarAndStun_transform();
        PlayerPrefs.SetInt("Stage", 100);
        //if (PlayerPrefs.GetInt("Stage") == 1) PlayerPrefs.SetInt("Stage", 1);
    }

    // Update is called once per frame
    void Update()
    {
        StageCount = PlayerPrefs.GetInt("Stage");
        if (NextStage) StageChange(StageCount);

        if (MonsterCount == MonsterDeadCount)
        {
            if (nullAndinput) deckField_null();
            else
            {
                if (StageCardInput)
                {
                    StageCard_inField(StageCount);
                    turn = false;
                    StageCardInput = false;
                }
            }
        }
        else
        {
            if (deckField.DeckField_nowCard == 5) turn = true;
            if (turn)
            {
                if (deckField.DeckField_nowCard == 1)
                {
                    turnEnd_prefab.SetActive(enabled);
                    turn = false;
                }
            }
        }
    }
    void StageCard_inField(int stage)
    {
        switch (stage)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 100:
                deckField.DeckField_nowCard++;
                deckField.rolling = true;
                card = Instantiate(StageCard[0], deck.transform.position, Quaternion.identity);
                deckField.Card_inField_Script[0] = card.GetComponent<Card_Script>();
                deckField.CardCode[0] = "다음으로";
                deckField.Card_inField_Script[0].Card_name = "다음으로";
                deckField.Card_inField_Script[0].Card_Number = 0;
                deckField.Card_inField[0] = card;
                break;
            default:
                break;
        }
    }
    void deckField_null()
    {
        for (int i = 0; i < deckField.Card_inField.Length; i++)
        {
            Destroy(deckField.Card_inField[i]);
            deckField.Card_inField_Script[i] = null;
            deckField.CardCode[i] = null;
            deckField.CardStatus[i] = 0;
        }
        deckField.DeckField_nowCard = 0;
        StageCardInput = true;
        nullAndinput = false;
    }
    [SerializeField] int a = 0;
    void StageChange(int Stage)
    {
        switch(Stage)
        {
            
            case 100:

                //Field_transform[0] = new Vector3(-5, Monster_Object[a].transform.position.y, 5);
                //Field_transform[1] = new Vector3(2, Monster_Object[a + 1].transform.position.y, 5);
                //Field_transform[2] = new Vector3(9, Monster_Object[a + 2].transform.position.y, 5);
                //Field_transform[3] = new Vector3(16, Monster_Object[a + 3].transform.position.y, 5);

                //Field_inMonster[0] = Instantiate(Monster_Object[a], Field_transform[0], Quaternion.identity);
                //Field_inMonster[1] = Instantiate(Monster_Object[a + 1], Field_transform[1], Quaternion.identity);
                //Field_inMonster[2] = Instantiate(Monster_Object[a + 2], Field_transform[2], Quaternion.identity);
                //Field_inMonster[3] = Instantiate(Monster_Object[a + 3], Field_transform[3], Quaternion.identity);

                //Enemy_Name[0] = Monster_Object[a].name;
                //Enemy_Name[1] = Monster_Object[a + 1].name;
                //Enemy_Name[2] = Monster_Object[a + 2].name;
                //Enemy_Name[3] = Monster_Object[a + 3].name;

                //MonsterCount = 4;
                //MonsterDeadCount = 0;
                //nullAndinput = true;

                Field_transform[2] = new Vector3(9, Monster_Object[a].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[a], Field_transform[2], Quaternion.identity);
                Enemy_Name[2] = Monster_Object[a].name;

                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;

            case 1: // 마을 중앙

                Field_transform[0] = new Vector3(-5, Monster_Object[0].transform.position.y, 5);
                Field_transform[1] = new Vector3(2, Monster_Object[1].transform.position.y, 5);
                Field_transform[2] = new Vector3(9, Monster_Object[2].transform.position.y, 5);
                Field_transform[3] = new Vector3(16, Monster_Object[3].transform.position.y, 5);

                Field_inMonster[0] = Instantiate(Monster_Object[0], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[1], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[2], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[3], Field_transform[3], Quaternion.identity);

                //Enemy_Name[0] = Monster_Object[a].name;
                //Enemy_Name[1] = Monster_Object[a].name;
                //Enemy_Name[2] = Monster_Object[a].name;
                //Enemy_Name[3] = Monster_Object[a].name;

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 2: // 마을 내곽

                Field_transform[0] = new Vector3(-5, Monster_Object[4].transform.position.y, 5);
                Field_transform[1] = new Vector3(2, Monster_Object[5].transform.position.y, 5);
                Field_transform[2] = new Vector3(9, Monster_Object[6].transform.position.y, 5);
                Field_transform[3] = new Vector3(16, Monster_Object[7].transform.position.y, 5);

                Field_inMonster[0] = Instantiate(Monster_Object[4], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[5], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[6], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[7], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "BlackSlime";
                Enemy_Name[1] = "DeathBringer";
                Enemy_Name[2] = "FireWizard";
                Enemy_Name[3] = "Necromancer";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 3: // 마을 외곽

                Field_transform[0] = new Vector3(-5, Monster_Object[8].transform.position.y, 5);
                Field_transform[1] = new Vector3(2, Monster_Object[9].transform.position.y, 5);
                Field_transform[2] = new Vector3(9, Monster_Object[10].transform.position.y, 5);
                Field_transform[3] = new Vector3(16, Monster_Object[11].transform.position.y, 5);

                Field_inMonster[0] = Instantiate(Monster_Object[8], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[9], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[10], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[11], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "FireWorm";
                Enemy_Name[1] = "FlyDemon";
                Enemy_Name[2] = "miniMushroom";
                Enemy_Name[3] = "Bat";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 4: // 숲 외곽

                Field_transform[0] = new Vector3(-5, Monster_Object[12].transform.position.y, 5);
                Field_transform[1] = new Vector3(2, Monster_Object[13].transform.position.y, 5);
                //Field_transform[2] = new Vector3(9, Monster_Object[14].transform.position.y, 5);
                //Field_transform[3] = new Vector3(16, Monster_Object[15].transform.position.y, 5);

                Field_inMonster[0] = Instantiate(Monster_Object[12], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[13], Field_transform[1], Quaternion.identity);
                //Field_inMonster[2] = Instantiate(Monster_Object[14], Field_transform[2], Quaternion.identity);
                //Field_inMonster[3] = Instantiate(Monster_Object[15], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "OrangeGolem";
                Enemy_Name[1] = "BlueGolem";
                Enemy_Name[2] = "";
                Enemy_Name[3] = "";

                MonsterCount = 2;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 5: // 밝은 숲

                Field_transform[2] = new Vector3(9, Monster_Object[14].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[14], Field_transform[2], Quaternion.identity);
                Enemy_Name[2] = "Titan";
                
                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 6: // 울창한 숲
                Field_transform[2] = new Vector3(9, Monster_Object[15].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[15], Field_transform[2], Quaternion.identity);
                Enemy_Name[2] = "DemonSlime";

                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 7: // 깊은 숲

                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 8: // 동굴이 있는 숲
                Field_transform[2] = new Vector3(9, Monster_Object[17].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[17], Field_transform[2], Quaternion.identity);
                Enemy_Name[2] = "AxeCastleGuardian";

                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 9: // 동굴 입구
                Field_transform[2] = new Vector3(9, Monster_Object[18].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[18], Field_transform[2], Quaternion.identity);
                Enemy_Name[2] = "SpearCastleGuardian";

                MonsterCount = 1;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 10: // 중심층
                break;
            case 11: // 심층 동굴
                break;
            case 12: // 어디론가 연결된 다리
                break;
            case 13: // 다리의 끝
                Field_transform[2] = new Vector3(9, Monster_Object[29].transform.position.y, 5);
                Field_inMonster[2] = Instantiate(Monster_Object[29], Field_transform[2], Quaternion.identity);

                Enemy_Name[2] = Monster_Object[29].name;
                break;
            case 14: // 성 외곽
                break;
            case 15: // 복도
                break;
            case 16: // 마지막
                    break;

            default:
                break;
        }
        NextStage = false;
    }
    void HpbarAndStun_transform()
    {
        for(int i = 0; i < 4; i++) if (EnemyHpbar[i] == null)
            {
                EnemyHpbar[i] = Instantiate(EnemyHpbar_prefab, canvas.transform).GetComponent<RectTransform>();
                EnemyStun[i] = Instantiate(EnemyStun_prefab, canvas.transform).GetComponent<RectTransform>();
                EnemyHpbar[i].gameObject.SetActive(false);
                EnemyStun[i].gameObject.SetActive(false);
            }

        if (EnemyBossHpbar == null)
        {
            EnemyBossHpbar = Instantiate(EnemyBossHpbar_prefab, canvas.transform).GetComponent<RectTransform>();
            EnemyBossHpbar.gameObject.SetActive(false);
        }
    }
    public void NextStageInput()
    {
        StageCardInput = true;
        int StageUp = PlayerPrefs.GetInt("Stage");
        StageUp++;
        PlayerPrefs.SetInt("Stage", StageUp);
        NextStage = true;
    }
}
