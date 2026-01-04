using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

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
    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
        //if (PlayerPrefs.GetInt("Stage") == 1) PlayerPrefs.SetInt("Stage", 1);
        Field_transform[0] = new Vector3(-5, 2, 5);
        Field_transform[1] = new Vector3(2, 2, 5);
        Field_transform[2] = new Vector3(9, 2, 5);
        Field_transform[3] = new Vector3(16, 2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        StageCount = PlayerPrefs.GetInt("Stage");
        if (NextStage) StageChange(StageCount);

        if (MonsterCount == MonsterDeadCount)
        {
            if(nullAndinput) deckField_null();
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
                if (deckField.DeckField_nowCard == 0)
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
    void StageChange(int Stage)
    {
        switch(Stage)
        {
            case 1:
                Field_inMonster[0] = Instantiate(Monster_Object[0], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[1], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[2], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[3], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Skeleton";
                Enemy_Name[1] = "Eye";
                Enemy_Name[2] = "Goblin";
                Enemy_Name[3] = "Mushroom";

                MonsterCount = 4;
                break;
            case 2:
                
                Field_inMonster[0] = Instantiate(Monster_Object[0], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[0], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[0], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[0], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Skeleton";
                Enemy_Name[1] = "Skeleton";
                Enemy_Name[2] = "Skeleton";
                Enemy_Name[3] = "Skeleton";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 3:

                Field_inMonster[0] = Instantiate(Monster_Object[1], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[1], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[1], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[1], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Eye";
                Enemy_Name[1] = "Eye";
                Enemy_Name[2] = "Eye";
                Enemy_Name[3] = "Eye";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 4:

                Field_inMonster[0] = Instantiate(Monster_Object[2], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[2], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[2], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[2], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Goblin";
                Enemy_Name[1] = "Goblin";
                Enemy_Name[2] = "Goblin";
                Enemy_Name[3] = "Goblin";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            case 5:

                Field_inMonster[0] = Instantiate(Monster_Object[3], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[3], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[3], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[3], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Mushroom";
                Enemy_Name[1] = "Mushroom";
                Enemy_Name[2] = "Mushroom";
                Enemy_Name[3] = "Mushroom";

                MonsterCount = 4;
                MonsterDeadCount = 0;
                nullAndinput = true;
                break;
            default:
                break;
        }
        NextStage = false;
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
