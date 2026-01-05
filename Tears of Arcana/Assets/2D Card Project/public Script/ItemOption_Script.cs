using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemOption_Script : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StatusText;

    [SerializeField] GameObject[] itemCard_prefab;
    public GameObject[] StatusCard_Object;
    public ItemCard_Script ClickItem;
    [SerializeField] GameObject[] InField_nowitemCard;
    public int Field_nowCardCount = 0;

    [SerializeField] GameObject Target_prefab;
    public GameObject[] target_Field;

    [SerializeField] Vector3[] item_transform;

    [SerializeField] Player_Script player;
    [SerializeField] CardDeckField_Script deckField;
    [SerializeField] int HpPlus_Persent = 0;
    [SerializeField] int MpPlus_Persent = 0;
    [SerializeField] string Attack_Persent = "";
    [SerializeField] int Defence_Persent = 0;
    [SerializeField] int Avoid_Persent = 0;

    public bool CardFieldActive = true;
    public bool item_inFieldCard = true;

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

        for(int j =0; j < itemCard_prefab.Length; j++)
        {
            InField_nowitemCard[j] = Instantiate(itemCard_prefab[j], spawnPosition, Quaternion.identity);
            Field_nowCardCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpDateStatusText();
        if (CardFieldActive) CardDeck_Active();
        if (item_inFieldCard) itemCard_inField();
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

        for (int i = 0; i < Field_nowCardCount; i++)
        {
            if (InField_nowitemCard[i] != null) // 카드가 생성이 되었을 때
            {
                if (!InField_nowitemCard[i].activeSelf) InField_nowitemCard[i].SetActive(true);
                InField_nowitemCard[i].transform.position = Vector3.Lerp(InField_nowitemCard[i].transform.position,
                    targetPosition[i], 2.5f * Time.deltaTime); // 적용
            }
        }
    }
    public void itemCard_ActiveAndTransform()
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

        for (int i = 0; i < Field_nowCardCount; i++)
        {
            if (InField_nowitemCard[i] != null) // 카드가 생성이 되었을 때
            {
                if (InField_nowitemCard[i].activeSelf) InField_nowitemCard[i].SetActive(false);
                InField_nowitemCard[i].transform.position = targetPosition[i]; // 적용
            }
        }
    }
    void CardDeck_Active()
    {
        for (int i = 0; i < 5; i++)
        {
            if (deckField.Card_inField[i].activeSelf) deckField.Card_inField[i].SetActive(false);
            else
            {
                deckField.Card_inField[i].SetActive(true);
                if (i == 4) this.gameObject.SetActive(false);
            }
        }
        CardFieldActive = false;
    }
    public void BoxColliderActive(bool box)
    {
        if(box) for (int i = 0; i < 3; i++) GameObject.Find("StatusCard0" + i).GetComponent<BoxCollider2D>().enabled = true;
        else for (int i = 0; i < 3; i++) GameObject.Find("StatusCard0" + i).GetComponent<BoxCollider2D>().enabled = false;
    }
    void UpDateStatusText()
    {
        StatusText.text = 
            player.nowHp + " / " + player.maxHp + " ( +" + HpPlus_Persent + " )\n" +
            player.nowMp + " / " + player.maxMp + " ( +" + MpPlus_Persent + " )\n" +
            "카드의 공격력" + Attack_Persent + "\n" +
            "20% / " + (20 + Defence_Persent) + "% ( " + Defence_Persent + "% )\n" +
            "10% / " + (10 + Avoid_Persent) + "% ( " + Avoid_Persent + "% )";
    }
}
