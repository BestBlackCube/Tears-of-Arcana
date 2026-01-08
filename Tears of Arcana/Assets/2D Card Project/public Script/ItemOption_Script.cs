using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ItemOption_Script : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StatusText;

    [SerializeField] GameObject[] itemCard_prefab;
    public GameObject[] StatusCard_Object;
    public ItemCard_Script ClickItem;
    public GameObject[] InField_nowitemCard;
    public int Field_nowCardCount = 0;

    [SerializeField] GameObject Target_prefab;
    public GameObject[] target_Field;

    [SerializeField] Vector3[] item_transform;

    [SerializeField] Player_Script player;
    [SerializeField] CardDeckField_Script deckField;
    public int HpPlus_Persent = 0;
    public int MpPlus_Persent = 0;
    public string Attack_Persent = "";
    public int Defence_Persent = 0;
    public int Avoid_Persent = 0;

    Vector3 spawnPosition = new Vector3(19.25f, -8f, -1);
    Vector3 FieldPosition = new Vector3(0, -8f, -1);
    Vector3[] targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            item_transform[i] = GameObject.Find("StatusCard0" + i).transform.position;
            target_Field[i] = Instantiate(Target_prefab, item_transform[i], Quaternion.identity);

            EnemyTargetBar_Script guide = target_Field[i].GetComponent<EnemyTargetBar_Script>();
            guide.target = GameObject.Find("StatusCard0" + i).transform;

            guide.offset[0] = new Vector3(-1.3f, 2.3f, -1);
            guide.offset[1] = new Vector3(1.3f, 2.3f, -1);
            guide.offset[2] = new Vector3(-1.3f, -2.3f, -1);
            guide.offset[3] = new Vector3(1.3f, -2.3f, -1);

            target_Field[i].SetActive(false);

        }

        BoxColliderActive(false);

        for(int j = 0; j < itemCard_prefab.Length; j++)
        {
            InField_nowitemCard[j] = Instantiate(itemCard_prefab[j], spawnPosition, Quaternion.identity);
            Field_nowCardCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpDateStatusText();
        itemCard_inField();
    }
    void itemCard_inField()
    {
        int count = Field_nowCardCount; // 카드 배열의 크기
        if (count == 0) return; // 카드가 존재하지 않다면 실행을 멈춤
        targetPosition = new Vector3[count];

        for (int i = 0, index = 0; i < count; i++) // 현재 카드필드에 있는 배열의 크기만큼 반복
        {
            if (InField_nowitemCard[i] != null) // 오브젝트가 있을 경우
            {
                float offset = (index - (count - 1) / 2f) * 3.5f; // 카드의 위치 = [반복인덱스] - [카드필드 배열 크기] / 2 * [카드간의 거리]
                targetPosition[i] = FieldPosition + new Vector3(offset, 0, -1); // x값 대입
                index++; // 반복 인덱스
            }
        }
        if (!InField_nowitemCard[0].GetComponent<ItemCard_Script>().Battle_Active)
        {
            if (InField_nowitemCard[0] != null)
            {
                if (!InField_nowitemCard[0].activeSelf) InField_nowitemCard[0].SetActive(true);
                InField_nowitemCard[0].transform.position = Vector3.Lerp(InField_nowitemCard[0].transform.position,
                    targetPosition[0], 2.5f * Time.deltaTime); // 적용
            }
        } 

        for (int i = 1; i < Field_nowCardCount; i++)
        {
            if (InField_nowitemCard[i] != null && !InField_nowitemCard[i].GetComponent<ItemCard_Script>().Card_Active) // 카드가 생성이 되었을 때
            {
                if (!InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status00 &&
                    !InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status01 &&
                    !InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status02)
                {
                    if (!InField_nowitemCard[i].activeSelf) InField_nowitemCard[i].SetActive(true);
                    InField_nowitemCard[i].transform.position = Vector3.Lerp(InField_nowitemCard[i].transform.position,
                        targetPosition[i], 2.5f * Time.deltaTime); // 적용
                }
                else
                {
                    if (InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status00)
                    {
                        InField_nowitemCard[i].transform.position = Vector3.Lerp(InField_nowitemCard[i].transform.position
                            , StatusCard_Object[0].transform.position, 2.5f * Time.deltaTime);
                    }
                    if (InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status01)
                    {
                        InField_nowitemCard[i].transform.position = Vector3.Lerp(InField_nowitemCard[i].transform.position
                            , StatusCard_Object[1].transform.position, 2.5f * Time.deltaTime);
                    }
                    if (InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status02)
                    {
                        InField_nowitemCard[i].transform.position = Vector3.Lerp(InField_nowitemCard[i].transform.position
                            , StatusCard_Object[2].transform.position, 2.5f * Time.deltaTime);
                    }
                }
            }
        }
    }
    public void itemCard_resetTransform()
    {
        for(int i = 0; i < InField_nowitemCard.Length; i++)
        {
            if (InField_nowitemCard[i] != null)
            {
                if (!InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status00 &&
                !InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status01 &&
                !InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status02)
                {
                    InField_nowitemCard[i].transform.position = spawnPosition;
                }
                InField_nowitemCard[i].SetActive(false);
            }
            if(i == InField_nowitemCard.Length - 1) this.gameObject.SetActive(false);
        }
    }
    public void CardDeck_Active()
    {
        for (int i = 0; i < deckField.DeckField_nowCard; i++)
        {
            if (deckField.Card_inField[i].activeSelf) deckField.Card_inField[i].SetActive(false);
            else deckField.Card_inField[i].SetActive(true);
        }
    }
    public void BoxColliderActive(bool box)
    {
        if(box) for (int i = 0; i < 3; i++) GameObject.Find("StatusCard0" + i).GetComponent<BoxCollider2D>().enabled = true;
        else for (int i = 0; i < 3; i++) GameObject.Find("StatusCard0" + i).GetComponent<BoxCollider2D>().enabled = false;
    }
    public void itemCard_ActiveOn()
    {
        for(int i = 0; i < 10; i++)
            if (InField_nowitemCard[i] != null)
            {
                InField_nowitemCard[i].SetActive(true);
                InField_nowitemCard[i].GetComponent<BoxCollider2D>().enabled = true;
            }
    }
    void UpDateStatusText()
    {
        StatusText.text = 
            player.nowHp + " / " + player.maxHp + " ( +" + HpPlus_Persent + " )\n" +
            player.nowMp + " / " + player.maxMp + " ( +" + MpPlus_Persent + " )\n" +
            "카드의 공격력" + Attack_Persent + "\n" +
            "20% / " + (player.Hit_percent - 10) + "% ( " + Defence_Persent + "% )\n" +
            "10% / " + (player.Defence_percent) + "% ( " + Avoid_Persent + "% )";
    }
}
