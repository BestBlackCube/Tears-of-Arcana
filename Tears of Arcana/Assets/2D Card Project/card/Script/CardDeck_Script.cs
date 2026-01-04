using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck_Script : MonoBehaviour
{
    CardDeckField_Script deckField;

    public GameObject[] CardData;
    public Card_namedata[] cardname;
    public card_Status status;
    public Sprite[] CardSprite;

    GameObject Card_Data;

    public bool Cardinput = false;
    public int CardCount = 0;
    public float rolling_timer = 0f;
    public float Cardinput_timer = 0f;

    public card_Status Idle_healthPotionCard_status = new card_Status();
    public card_Status High_healthPotionCard_status = new card_Status();
    public card_Status MeditationCard_status = new card_Status();
    public card_Status MagicCard_status = new card_Status();
    public card_Status FireCard_status = new card_Status();
    public card_Status WaterCard_status = new card_Status();
    public card_Status WindCard_status = new card_Status();
    public card_Status SoilCard_status = new card_Status();
    public card_Status FireOfvitalityCard_status = new card_Status();
    public card_Status QuietrestCard_status = new card_Status();
    public card_Status AbyssCreviceCard_status = new card_Status();
    public card_Status BrutalContractCard_status = new card_Status();

    // Start is called before the first frame update
    void Start()
    {
        deckField = FindObjectOfType<CardDeckField_Script>();
        status = new card_Status();

        Idle_healthPotionCard_status = status.Card_inStatus(cardname[0]);
        High_healthPotionCard_status = status.Card_inStatus(cardname[1]);
        MeditationCard_status = status.Card_inStatus(cardname[2]);
        MagicCard_status = status.Card_inStatus(cardname[3]);
        FireCard_status = status.Card_inStatus(cardname[4]);
        WaterCard_status = status.Card_inStatus(cardname[5]);
        WindCard_status = status.Card_inStatus(cardname[6]);
        SoilCard_status = status.Card_inStatus(cardname[7]);
        FireOfvitalityCard_status = status.Card_inStatus(cardname[8]);
        QuietrestCard_status = status.Card_inStatus(cardname[9]);
        AbyssCreviceCard_status = status.Card_inStatus(cardname[10]);
        BrutalContractCard_status = status.Card_inStatus(cardname[11]);

    }
    /*
     
    카드, 스테이터스, 이미지 배열
    0. 하급회복포션 1. 상급회복포션 2. 명상

    3. 기본마법 4. 화염장판 5. 얼음 안개 6. 바람의창 7. 돌무더기

    8. 생명의잔불 9. 고요한안식 10. 절망의균열 11. 잔혹한계약
     
     */
    // Update is called once per frame
    void Update()
    {
        if(Cardinput)
        {
            if(CardCount == 5) CardCount = 0;
            if (deckField.DeckField_nowCard < 5) // 최대 5개까지 제한하는 조건문
            {
                if (Cardinput_timer < 1.2f) Cardinput_timer += Time.deltaTime; // 1.2초 마다 카드가 생성
                else
                {
                    int number = Random.Range(4, 6); // 테스트용 코드
                    if (number == 1)
                    {
                        Card_Data = Instantiate(CardData[0], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("하급회복물약");
                    }
                    if (number == 2)
                    {
                        Card_Data = Instantiate(CardData[1], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("상급회복물약");
                    }
                    if (number == 3)
                    {
                        Card_Data = Instantiate(CardData[2], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("명상");
                    }
                    if (number == 4)
                    {
                        Card_Data = Instantiate(CardData[3], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("일반마법");
                    }
                    if (number == 5)
                    {
                        Card_Data = Instantiate(CardData[4], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("화염장판");
                    }
                    if (number == 6)
                    {
                        Card_Data = Instantiate(CardData[5], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("얼음안개");
                    }
                    if (number == 7)
                    {
                        Card_Data = Instantiate(CardData[6], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("바람의창");
                    }
                    if (number == 8)
                    {
                        Card_Data = Instantiate(CardData[7], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("돌무더기");
                    }
                    if (number == 9)
                    {
                        Card_Data = Instantiate(CardData[8], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("생명의잔불");
                    }
                    if (number == 10)
                    {
                        Card_Data = Instantiate(CardData[9], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("고요한안식");
                    }
                    if (number == 11)
                    {
                        Card_Data = Instantiate(CardData[10], this.transform.position, Quaternion.identity); // 복제한 오브젝트
                        Public_Card_inputData("절망의균열");
                    }
                    if (number == 12)
                    {
                        Card_Data = Instantiate(CardData[11], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("잔혹한계약");
                    }
                    deckField.rolling = true;
                    CardCount++; // 뽑을수 있는 카드 인덱스 증가
                    deckField.DeckField_nowCard++; // 인덱스 증가
                    Cardinput_timer = 0f;
                }
            }
            else
            {
                Cardinput = false;
            }
        }
    }
    void Public_Card_inputData(string CardName)
    {
        deckField.Card_inField[CardCount] = Card_Data; // 오브젝트를 해당 카드필드 배열에 대입
        deckField.Card_inField_Script[CardCount] = Card_Data.GetComponent<Card_Script>();
        deckField.Card_inField_Script[CardCount].Card_Number = CardCount;
        switch(CardName)
        {
            case "하급회복물약":
                deckField.CardCode[CardCount] = Idle_healthPotionCard_status.InputName;
                deckField.CardStatus[CardCount] = Idle_healthPotionCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[0];

                deckField.Card_inField_Script[CardCount].Card_name = Idle_healthPotionCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = Idle_healthPotionCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = Idle_healthPotionCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = Idle_healthPotionCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = Idle_healthPotionCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = Idle_healthPotionCard_status.Count;
                break;
            case "상급회복물약":
                deckField.CardCode[CardCount] = High_healthPotionCard_status.InputName;
                deckField.CardStatus[CardCount] = High_healthPotionCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[1];

                deckField.Card_inField_Script[CardCount].Card_name = High_healthPotionCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = High_healthPotionCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = High_healthPotionCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = High_healthPotionCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = High_healthPotionCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = High_healthPotionCard_status.Count;
                break;
            case "명상":
                deckField.CardCode[CardCount] = MeditationCard_status.InputName;
                deckField.CardStatus[CardCount] = MeditationCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[2];

                deckField.Card_inField_Script[CardCount].Card_name = MeditationCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = MeditationCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = MeditationCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = MeditationCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = MeditationCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = MeditationCard_status.Count;
                break;
            case "일반마법":
                deckField.CardCode[CardCount] = MagicCard_status.InputName;
                deckField.CardStatus[CardCount] = MagicCard_status.Single_Damage;
                deckField.CardImage[CardCount] = CardSprite[3];

                deckField.Card_inField_Script[CardCount].Card_name = MagicCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = MagicCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = MagicCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = MagicCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = MagicCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = MagicCard_status.Count;
                break;
            case "화염장판":
                deckField.CardCode[CardCount] = FireCard_status.InputName;
                deckField.CardStatus[CardCount] = FireCard_status.Multiple_Damage;
                deckField.CardImage[CardCount] = CardSprite[4];

                deckField.Card_inField_Script[CardCount].Card_name = FireCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = FireCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = FireCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = FireCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = FireCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = FireCard_status.Count;
                break;
            case "얼음안개":
                deckField.CardCode[CardCount] = WaterCard_status.InputName;
                deckField.CardStatus[CardCount] = WaterCard_status.Multiple_Damage;
                deckField.CardImage[CardCount] = CardSprite[5];

                deckField.Card_inField_Script[CardCount].Card_name = WaterCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = WaterCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = WaterCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = WaterCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = WaterCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = WaterCard_status.Count;
                break;
            case "바람의창":
                deckField.CardCode[CardCount] = WindCard_status.InputName;
                deckField.CardStatus[CardCount] = WindCard_status.Single_Damage;
                deckField.CardImage[CardCount] = CardSprite[6];

                deckField.Card_inField_Script[CardCount].Card_name = WindCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = WindCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = WindCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = WindCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = WindCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = WindCard_status.Count;
                break;
            case "돌무더기":
                deckField.CardCode[CardCount] = SoilCard_status.InputName;
                deckField.CardStatus[CardCount] = SoilCard_status.Single_Damage;
                deckField.CardImage[CardCount] = CardSprite[7];

                deckField.Card_inField_Script[CardCount].Card_name = SoilCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = SoilCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = SoilCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = SoilCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = SoilCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = SoilCard_status.Count;
                break;
            case "생명의잔불":
                deckField.CardCode[CardCount] = FireOfvitalityCard_status.InputName;
                deckField.CardStatus[CardCount] = FireOfvitalityCard_status.Count;
                deckField.CardImage[CardCount] = CardSprite[8];

                deckField.Card_inField_Script[CardCount].Card_name = FireOfvitalityCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = FireOfvitalityCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = FireOfvitalityCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = FireOfvitalityCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = FireOfvitalityCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = FireOfvitalityCard_status.Count;
                break;
            case "고요한안식":
                deckField.CardCode[CardCount] = QuietrestCard_status.InputName;
                deckField.CardStatus[CardCount] = QuietrestCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[9];

                deckField.Card_inField_Script[CardCount].Card_name = QuietrestCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = QuietrestCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = QuietrestCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = QuietrestCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = QuietrestCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = QuietrestCard_status.Count;
                break;
            case "절망의균열":
                deckField.CardCode[CardCount] = AbyssCreviceCard_status.InputName;
                deckField.CardStatus[CardCount] = AbyssCreviceCard_status.Single_Damage;
                deckField.CardImage[CardCount] = CardSprite[10];

                deckField.Card_inField_Script[CardCount].Card_name = AbyssCreviceCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = AbyssCreviceCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = AbyssCreviceCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = AbyssCreviceCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = AbyssCreviceCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = AbyssCreviceCard_status.Count;
                break;
            case "잔혹한계약":
                deckField.CardCode[CardCount] = BrutalContractCard_status.InputName;
                deckField.CardStatus[CardCount] = BrutalContractCard_status.Single_Damage;
                deckField.CardImage[CardCount] = CardSprite[11];

                deckField.Card_inField_Script[CardCount].Card_name = BrutalContractCard_status.InputName;
                deckField.Card_inField_Script[CardCount].single_damage = BrutalContractCard_status.Single_Damage;
                deckField.Card_inField_Script[CardCount].multiple_damage = BrutalContractCard_status.Multiple_Damage;
                deckField.Card_inField_Script[CardCount].health = BrutalContractCard_status.Health;
                deckField.Card_inField_Script[CardCount].mana = BrutalContractCard_status.Mana;
                deckField.Card_inField_Script[CardCount].count = BrutalContractCard_status.Count;
                break;
            default:
                break;
        }
    }
}
