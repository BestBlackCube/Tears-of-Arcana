using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck_Script : MonoBehaviour
{
    CardDeckField_Script deckField;

    public GameObject[] CardData;
    public Card_namedata[] cardname;

    public Sprite[] CardSprite;

    GameObject Card_Data;

    public bool Cardinput = false;
    public int CardCount = 0;
    public float rolling_timer = 0f;
    public float Cardinput_timer = 0f;

    public card_Status Idle_healthPotionCard_status = new card_Status();
    public card_Status High_healthPotionCard_status = new card_Status();
    public card_Status Meditation = new card_Status();
    public card_Status attackCard_status = new card_Status();
    public card_Status magicCard_status = new card_Status();
    public card_Status fireCard_status = new card_Status();
    public card_Status waterCard_status = new card_Status();
    public card_Status windCard_status = new card_Status();
    public card_Status soilCard_status = new card_Status();
    public card_Status devilCard_status = new card_Status();

    // Start is called before the first frame update
    void Start()
    {
        deckField = FindObjectOfType<CardDeckField_Script>();

        Idle_healthPotionCard_status = Idle_healthPotionCard_status.Card_inStatus(cardname[0]);
        High_healthPotionCard_status= High_healthPotionCard_status.Card_inStatus(cardname[1]);
                      Meditation = Meditation.Card_inStatus(cardname[2]);
        attackCard_status = attackCard_status.Card_inStatus(cardname[3]);
          magicCard_status = magicCard_status.Card_inStatus(cardname[4]);
            fireCard_status = fireCard_status.Card_inStatus(cardname[5]);
          waterCard_status = waterCard_status.Card_inStatus(cardname[6]);
            windCard_status = windCard_status.Card_inStatus(cardname[7]);
            soilCard_status = soilCard_status.Card_inStatus(cardname[8]);
          devilCard_status = devilCard_status.Card_inStatus(cardname[9]);

    }
    /*
     
    카드 배열
    0. 회복 카드 1. 물리공격 카드 2. 마법공격 카드
    3. 불속성 카드 4. 물속성 카드 5. 바람속성 카드
    6. 흙속성 카드 7. 특수 카드

    스테이터스 배열
    0. 기본물약 1. 하이물약 2. 명상
    3. 물리공격 4. 마법공격
    5. 불속성 6. 물속성 7. 바람속성 8. 흙속성
    9. 특수

    이미지 배열
    0. 불속성 1. 흙속성 2. 물속성
    3. 기본물약 4. 하이물약 5. 명상
     
     */
    // Update is called once per frame
    void Update()
    {
        if(Cardinput)
        {
            if (deckField.DeckField_nowCard < 5) // 최대 5개까지 제한하는 조건문
            {
                if (Cardinput_timer < 1.2f) Cardinput_timer += Time.deltaTime; // 1.2초 마다 카드가 생성
                else
                {
                    int number = Random.Range(1, 9); // 테스트용 코드
                    if (number == 1)
                    {
                        Card_Data = Instantiate(CardData[0], this.transform.position, Quaternion.identity); // 복제한 회복 오브젝트
                        Public_Card_inputData("중급회복물약");
                    }
                    if (number == 2)
                    {
                        Card_Data = Instantiate(CardData[0], this.transform.position, Quaternion.identity); // 복제한 회복 오브젝트
                        Public_Card_inputData("상급회복물약");
                    }
                    if (number == 3)
                    {
                        Card_Data = Instantiate(CardData[0], this.transform.position, Quaternion.identity); // 복제한 회복 오브젝트
                        Public_Card_inputData("명상");
                    }
                    if (number == 4)
                    {
                        Card_Data = Instantiate(CardData[1], this.transform.position, Quaternion.identity); // 복제한 물리공격 오브젝트
                        Public_Card_inputData("일반물리공격");
                    }
                    if (number == 5)
                    {
                        Card_Data = Instantiate(CardData[2], this.transform.position, Quaternion.identity); // 복제한 마법공격 오브젝트
                        Public_Card_inputData("일반마법공격");
                    }
                    if (number == 6)
                    {
                        Card_Data = Instantiate(CardData[3], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("불속성카드");
                    }
                    if (number == 7)
                    {
                        Card_Data = Instantiate(CardData[4], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("물속성카드");
                    }
                    if (number == 8)
                    {
                        Card_Data = Instantiate(CardData[5], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("바람속성카드");
                    }
                    if (number == 9)
                    {
                        Card_Data = Instantiate(CardData[6], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("흙속성카드");
                    }
                    if (number == 10)
                    {
                        Card_Data = Instantiate(CardData[7], this.transform.position, Quaternion.identity); // 복제한 악마카드 오브젝트
                        Public_Card_inputData("특수카드");
                    }
                    deckField.rolling = true;
                    CardCount++; // 뽑을수 있는 카드 인덱스 증가
                    deckField.card_Setting = true; // 카드 위치 셋팅 하는 코드 실행
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
            case "중급회복물약":
                deckField.CardCode[CardCount] = Idle_healthPotionCard_status.InputName;
                deckField.CardStatus[CardCount] = Idle_healthPotionCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[0];
                break;
            case "상급회복물약":
                deckField.CardCode[CardCount] = High_healthPotionCard_status.InputName;
                deckField.CardStatus[CardCount] = High_healthPotionCard_status.Health;
                deckField.CardImage[CardCount] = CardSprite[1];
                break;
            case "명상":
                deckField.CardCode[CardCount] = Meditation.InputName;
                deckField.CardStatus[CardCount] = Meditation.Health;
                deckField.CardImage[CardCount] = CardSprite[2];
                break;
            case "일반물리공격":
                deckField.CardCode[CardCount] = attackCard_status.InputName;
                deckField.CardStatus[CardCount] = attackCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[3];
                break;
            case "일반마법공격":
                deckField.CardCode[CardCount] = magicCard_status.InputName;
                deckField.CardStatus[CardCount] = magicCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[3];
                break;
            case "불속성카드":
                deckField.CardCode[CardCount] = fireCard_status.InputName;
                deckField.CardStatus[CardCount] = fireCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[3];
                break;
            case "물속성카드":
                deckField.CardCode[CardCount] = waterCard_status.InputName;
                deckField.CardStatus[CardCount] = waterCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[4];
                break;
            case "바람속성카드":
                deckField.CardCode[CardCount] = windCard_status.InputName;
                deckField.CardStatus[CardCount] = windCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[3];
                break;
            case "흙속성카드":
                deckField.CardCode[CardCount] = soilCard_status.InputName;
                deckField.CardStatus[CardCount] = soilCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[5];
                break;
            case "특수카드":
                deckField.CardCode[CardCount] = devilCard_status.InputName;
                deckField.CardStatus[CardCount] = devilCard_status.Damage;
                deckField.CardImage[CardCount] = CardSprite[5];
                break;
            default:
                break;
        }
    }
}
