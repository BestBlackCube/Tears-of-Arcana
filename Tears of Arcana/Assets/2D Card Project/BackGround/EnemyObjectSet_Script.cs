using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyObjectSet_Script : MonoBehaviour
{
    public CardDeck_Script deck;
    public CardDeckField_Script deckField;
    public GameObject[] StageCard;
    GameObject card;
    public GameObject[] Monster_Object;
    public GameObject[] Field_inMonster;
    public string[] Enemy_Name;
    public bool NextStage = false;
    public bool StageCardInput = false;
    public int StageCount = 0;
    public int MonsterCount = 0;
    public int MonsterDeadCount = 0;

    public Vector3[] Field_transform;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
        Field_transform[0] = new Vector3(-5, 2, 5);
        Field_transform[1] = new Vector3(2, 2, 5);
        Field_transform[2] = new Vector3(9, 2, 5);
        Field_transform[3] = new Vector3(16, 2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        StageCount = PlayerPrefs.GetInt("Stage");
        if (NextStage)
        {
            StageChange(StageCount);
        }
        if (MonsterCount == MonsterDeadCount)
        {
            if(StageCardInput)
            {
                StageCard_inField(StageCount);
                StageCardInput = false;
            }
        }
    }
    void StageCard_inField(int stage)
    {
        switch (stage)
        {
            case 1:
                deckField_null();
                card = Instantiate(StageCard[0], deck.transform.position, Quaternion.identity);
                card.GetComponent<Card_Script>().Card_name = "다음으로";
                deckField.Card_inField[0] = card;
                deckField.Card_inField_Script[0] = card.GetComponent<Card_Script>();
                deckField.Card_inField_Script[0].Card_Number = 1;
                deckField.rolling = true;
                deckField.card_Setting = true; 
                deckField.DeckField_nowCard++; 
                break;
            default:
                break;
        }
    }
    void deckField_null()
    {
        if(deckField.DeckField_nowCard > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                Destroy(deckField.Card_inField[i]);
                deckField.Card_inField_Script[i] = null;
                deckField.CardCode[i] = null;
                deckField.CardStatus[i] = 0;
                deckField.DeckField_nowCard--;
            }
        }
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
                Field_inMonster[0] = Instantiate(Monster_Object[1], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[1], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[1], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[1], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Eye";
                Enemy_Name[1] = "Eye";
                Enemy_Name[2] = "Eye";
                Enemy_Name[3] = "Eye";
                break;
            default:
                break;
        }
        NextStage = false;
    }
}
