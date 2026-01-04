using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class CardDeckField_Script : MonoBehaviour
{
    public int DeckField_nowCard = 0;

    public GameObject turnEnd;

    public Card_Script Click_Card;
    public GameObject[] Card_inField;
    public Card_Script[] Card_inField_Script;
    public string[] CardCode;
    public int[] CardStatus;
    public Sprite[] CardImage;

    public bool cardHide = false;
    public bool cardMove_Rock = false;
    public bool card_Setting = false;
    public bool newField = false;
    public bool rolling = false;

    public float rolling_timer = 0f;
    public Quaternion start = Quaternion.Euler(0, 0, 0);
    public Quaternion end = Quaternion.Euler(0, 360, 0);

    bool turn = false;
    float space = 3.5f;

    Vector3 basePosition = new Vector3(-1, -8.25f, 0);
    Vector3[] targetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cardHide) // 카드 숨기기
        {
            for (int i = 0; i < Card_inField.Length; i++) // 배열에 든 모든 카드를 화면 밖으로 이동시킨다
            {
                if (Card_inField_Script[i] != null && !Card_inField_Script[i].Card_noHide)
                {
                    if (Card_inField_Script[i].transform.position.y > -20) Card_inField_Script[i].transform.position = 
                            new Vector3(Card_inField_Script[i].transform.position.x,Card_inField_Script[i].transform.position.y - 20f * Time.deltaTime, 0);
                }
            }
        }
        else
        {
            Card_newPosition(); // 카드 위치 조정 실행
        }

        if (!cardMove_Rock) // 카드를 움직이지 않게 막는 조건문
        {
            if (DeckField_nowCard == 5) turn = true;
            if(turn)
            {
                if(DeckField_nowCard == 0)
                {
                    turnEnd.SetActive(enabled);
                    turn = false;
                }
            }
            for (int i = 0; i < DeckField_nowCard; i++)
            {
                if (Card_inField[i] != null) // 카드가 생성이 되었을 때
                {
                    Card_inField[i].transform.position = Vector3.Lerp(Card_inField[i].transform.position, targetPosition[i], 2.5f * Time.deltaTime); // 적용
                }
            }
            if (DeckField_nowCard > 0) // 카드가 필드에 있을 경우
            {
                if (rolling)
                {
                    rolling_timer += Time.deltaTime;
                    Card_inField[DeckField_nowCard - 1].transform.Rotate(new Vector3(0, 360, 0) * Time.deltaTime);

                    if (rolling_timer > 1f) rolling = false;
                }
                else
                {
                    Card_inField[DeckField_nowCard - 1].transform.rotation = Quaternion.Euler(0, 0, 0);
                    rolling_timer = 0f;
                }
            }
        }
    }
    void Card_newPosition() // 카드간의 거리
    {
        int count = DeckField_nowCard; // 카드 배열의 크기
        if (count == 0) return; // 카드가 존재하지 않다면 실행을 멈춤
        targetPosition = new Vector3[count];

        for (int i = 0, index = 0; i < count; i++) // 현재 카드필드에 있는 배열의 크기만큼 반복
        {
            if (Card_inField[i] != null) // 오브젝트가 있을 경우
            {
                float offset = (index - (count - 1) / 2f) * space; // 카드의 위치 = [반복인덱스] - [카드필드 배열 크기] / 2 * [카드간의 거리]
                targetPosition[i] = basePosition + new Vector3(offset, 0, 0); // x값 대입
                index++; // 반복 인덱스
            }
        }
        Card_inputCardName();
    }
    public void deckField_Null(int i) // Missing 상태를 None상태로 바꾸기
    {
        Card_inField[i] = null; // 카드를 파괴하면 None상태가 아닌 missing상태이므로 None상태로 초기화
        Card_inField_Script[i] = null; // <-
        CardCode[i] = null;
        CardStatus[i] = 0;
        New_deckField();
    }
    void New_deckField() // 빈 배열 채우기
    {
        for (int i = 0; i < Card_inField.Length; i++)
        {
            if (Card_inField[i] == null)
            {
                for (int j = i + 1; j < Card_inField.Length; j++)
                {
                    if (Card_inField[j] != null)
                    {
                        Card_inField[i] = Card_inField[j]; // 오브젝트
                        Card_inField[j] = null;

                        Card_inField_Script[i] = Card_inField_Script[j]; // 스크립트
                        Card_inField_Script[j] = null;

                        Card_inField_Script[i].Card_Number -= 1; // 스크립트에 있는 카드 번호

                        CardCode[i] = CardCode[j]; // 카드 이름
                        CardCode[j] = null;

                        CardStatus[i] = CardStatus[j]; // 카드 스텟
                        CardStatus[j] = 0;
                        break;
                    }
                }
            }
        }
        Card_inputCardName();
    }
    void Card_inputCardName() // 카드에 있는 TMP의 주소 값
    {
        for (int i = 0; i < DeckField_nowCard; i++)
        {
            if (Card_inField[i] != null)
            {
                CardInfoStatus_Script statusText = Card_inField[i].GetComponent<CardInfoStatus_Script>();
                statusText.CardName.text = CardCode[i];
                statusText.CardStatus.text = "" + CardStatus[i];
                statusText.ImageSet = CardImage[i];
            }
        }
    }
}
