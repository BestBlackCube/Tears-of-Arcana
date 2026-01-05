using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Card_Script : MonoBehaviour
{
    /*
     
    vector3 mouseposition = input.mouseposition; //마우스 좌표값 가져오기
    vector3 mouseworldposition = camera.main.screentoworldpoint(input.mouseposition); // 스크린 좌표를 월드 좌표로 변환하기
    vector3 mouseworldposition = camera.main.screentoworldpoint(new vector3(input.mouseposition.x, input.mouseposition.y, 0));
     //2D게임에서의 마우스 좌표 사용 예시

     */
    Player_Script player;
    CardDeckField_Script deckField;
    EnemyObjectSet_Script ObjectSet;

    Skeleton_Script skeleton;
    Eye_Script eye;
    Goblin_Script goblin;
    Mushroom_Script mushroom;

    BoxCollider2D box2D;
    card_Status status;

    public bool Card_MouseClick = false;
    public bool Card_noHide = false;
    public bool Box2D_Create = false;

    public Vector3 Card_transform;
    public int Card_upNumber;
    public int Card_Number = 0;

    public string Card_name;
    public int single_damage;
    public int multiple_damage;
    public int health;
    public int mana;
    public int count;

    public int Card_status;
    public string Object_name;

    float Mouse_insideCard_timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Script>();
        skeleton = FindObjectOfType<Skeleton_Script>();
        eye = FindObjectOfType<Eye_Script>();
        goblin = FindObjectOfType<Goblin_Script>();
        mushroom = FindObjectOfType<Mushroom_Script>();

        ObjectSet = FindObjectOfType<EnemyObjectSet_Script>();
        deckField = FindObjectOfType<CardDeckField_Script>();
        box2D = GetComponent<BoxCollider2D>();

        status = new card_Status();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && Card_MouseClick) // 마우스 우클릭과 마우스왼클릭이 작동을 했는지 감지하는 조건문
        {
            Card_MouseClick = false;         // 마우스 왼클릭 감지 해제
            Target_Card(Card_MouseClick);
            Box2D_Create = true;             // 파괴한 박스콜라이더 다시 생성
            deckField.cardMove_Rock = false; // 필드에 있는 카드 움직임을 멈추는 코드 해제
            deckField.cardHide = false;      // 카드배열 숨기기 해제
            Card_noHide = false;             // 카드배열 숨기기 막기 해제 
            Object_name = "";                // 이름 초기화
            deckField.Click_Card = null;
        }
        if (Card_MouseClick) // 왼클릭을 했는지 감지하는 조건문
        {
            Destroy(box2D);  // 박스콜라이더 파괴
            Object_inName(); // 카드 이름에 따라 오브젝트 이동
        }
        else // 왼클릭을 안했을 경우
        {
            if (box2D == null && Box2D_Create)
            {
                box2D = gameObject.AddComponent<BoxCollider2D>();
                Box2D_Create = false;
            }
        }
    }
    public void CardDestroy()
    {
        deckField.deckField_Null(Card_Number); // 배열에 있는 데이터 지우기
        deckField.cardMove_Rock = false;       // 카드를 들었을때 필드에있는 카드의 움직임을 멈추는 코드 해제
        deckField.DeckField_nowCard--;         // 필드에있는 카드수 1차감
        Destroy(this.gameObject);              // 오브젝트 파괴
    }

    private void OnMouseOver() // 마우스 커서가 해당 오브젝트의 BoxCollider2D 안에 있는지 확인하는 이벤트
    {
        if (Mouse_insideCard_timer < 1f) Mouse_insideCard_timer += Time.deltaTime;
        else
        {
            CardScale(true); // 2초 이상 해당 오브젝트에 마우스 커서가 있을 경우
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        }
    }
    private void OnMouseExit() // 마우스 커서가 해당 오브젝트의 BoxCollider2D 밖으로 나갔는지 확인하는 이벤트
    {
        CardScale(false); // 크기 줄이기
        Mouse_insideCard_timer = 0f; // 타이머 초기화\
    }
    private void OnMouseDown() // 마우스 커서가 해당 오브젝트의 BoxCollider2D 안에서 왼 클릭을 했는지 확인하는 이벤트
    {
        if(!Card_MouseClick)
        {
            Card_MouseClick = true;         // 카드 선택
            Card_noHide = true;             // 선택한 카드는 숨기지 못하게 막기
            deckField.cardMove_Rock = true; // 카드 움직임 멈추기
            deckField.cardHide = true;      // 카드 숨기기
            deckField.Click_Card = this.gameObject.GetComponent<Card_Script>();
            Target_Card(Card_MouseClick);
        }
    }
    void CardScale(bool Active) // 카드에 마우스 커서를 올렸을 경우
    {
        if(Active) transform.localScale = new Vector3(1.5f, 1.5f, 1); // 크기 키우기
        else transform.localScale = new Vector3(1, 1, 1); // 기본 크기
    }
    public void Target_Card(bool Click) // 사용 가능한 캐릭터
    {
        if(Click)
        {
            switch (Card_name)
            {
                case "다음으로":
                case "이전으로":
                case "하급회복물약":
                case "상급회복물약":
                case "명상":
                case "생명의잔불":
                case "고요한안식":
                case "잔혹한계약":
                    player.targetPlayerCard = true;
                    if (!player.Arrow) player.Arrow = true;
                    break;

                case "일반마법":
                case "바람의창":
                case "돌무더기":
                case "절망의균열":
                    EnemysingleTarget(true);
                    break;

                case "화염장판":
                case "얼음안개":
                    EnemymultipleTarget(true);
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (Card_name)
            {
                case "다음으로":
                case "이전으로":
                case "하급회복물약":
                case "상급회복물약":
                case "명상":
                case "생명의잔불":
                case "고요한안식":
                case "잔혹한계약":
                    player.targetPlayerCard = false;
                    if(player.Arrow) player.Arrow = false;
                    break;

                case "일반마법":
                case "바람의창":
                case "돌무더기":
                case "절망의균열":
                    EnemysingleTarget(false);
                    break;

                case "화염장판":
                case "얼음안개":
                    EnemymultipleTarget(false);
                    break;

                default:
                    break;
            }
        }

    }
    void EnemysingleTarget(bool TF)
    {
        if(TF)
        {
            if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
            ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Skeleton") 
                { 
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Arrow = true; 
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Guide = true; 
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().targetCard = true; 
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy) 
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy = true; 
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
                ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Eye")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Eye")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Eye")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Eye")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy)
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
                ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Goblin")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy)
                         ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Goblin")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy)
                         ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Goblin")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy)
                         ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Goblin")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy)
                         ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
                ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlackSlime" || ObjectSet.Enemy_Name[1] == "BlackSlime" ||
                ObjectSet.Enemy_Name[2] == "BlackSlime" || ObjectSet.Enemy_Name[3] == "BlackSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DeathBringer" || ObjectSet.Enemy_Name[1] == "DeathBringer" ||
                ObjectSet.Enemy_Name[2] == "DeathBringer" || ObjectSet.Enemy_Name[3] == "DeathBringer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWizard" || ObjectSet.Enemy_Name[1] == "FireWizard" ||
                ObjectSet.Enemy_Name[2] == "FireWizard" || ObjectSet.Enemy_Name[3] == "FireWizard")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Necromancer" || ObjectSet.Enemy_Name[1] == "Necromancer" ||
                ObjectSet.Enemy_Name[2] == "Necromancer" || ObjectSet.Enemy_Name[3] == "Necromancer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWorm" || ObjectSet.Enemy_Name[1] == "FireWorm" ||
                ObjectSet.Enemy_Name[2] == "FireWorm" || ObjectSet.Enemy_Name[3] == "FireWorm")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FlyDemon" || ObjectSet.Enemy_Name[1] == "FlyDemon" ||
                ObjectSet.Enemy_Name[2] == "FlyDemon" || ObjectSet.Enemy_Name[3] == "FlyDemon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "miniMushroom" || ObjectSet.Enemy_Name[1] == "miniMushroom" ||
                ObjectSet.Enemy_Name[2] == "miniMushroom" || ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Bat" || ObjectSet.Enemy_Name[1] == "Bat" ||
                ObjectSet.Enemy_Name[2] == "Bat" || ObjectSet.Enemy_Name[3] == "Bat")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Bat")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Bat")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Bat")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Bat")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "OrangeGolem" || ObjectSet.Enemy_Name[1] == "OrangeGolem" ||
                ObjectSet.Enemy_Name[2] == "OrangeGolem" || ObjectSet.Enemy_Name[3] == "OrangeGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlueGolem" || ObjectSet.Enemy_Name[1] == "BlueGolem" ||
                ObjectSet.Enemy_Name[2] == "BlueGolem" || ObjectSet.Enemy_Name[3] == "BlueGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior01" || ObjectSet.Enemy_Name[1] == "GhostWarrior01" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior01" || ObjectSet.Enemy_Name[3] == "GhostWarrior01")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior02" || ObjectSet.Enemy_Name[1] == "GhostWarrior02" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior02" || ObjectSet.Enemy_Name[3] == "GhostWarrior02")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior03" || ObjectSet.Enemy_Name[1] == "GhostWarrior03" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior03" || ObjectSet.Enemy_Name[3] == "GhostWarrior03")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior04" || ObjectSet.Enemy_Name[1] == "GhostWarrior04" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior04" || ObjectSet.Enemy_Name[3] == "GhostWarrior04")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
            }

            // BOSS

            if (ObjectSet.Enemy_Name[0] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[1] == "AxeCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[1] == "SpearCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Titan" || ObjectSet.Enemy_Name[1] == "Titan" ||
                ObjectSet.Enemy_Name[2] == "Titan" || ObjectSet.Enemy_Name[3] == "Titan")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Titan")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Titan")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Titan")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Titan")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DemonSlime" || ObjectSet.Enemy_Name[1] == "DemonSlime" ||
                ObjectSet.Enemy_Name[2] == "DemonSlime" || ObjectSet.Enemy_Name[3] == "DemonSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Argon" || ObjectSet.Enemy_Name[1] == "Argon" ||
                ObjectSet.Enemy_Name[2] == "Argon" || ObjectSet.Enemy_Name[3] == "Argon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Argon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Argon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Argon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Argon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().targetCard = true;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
            }
        }
        else
        {
            if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
                ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
                ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Eye")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Eye")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Eye")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Eye")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
                ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Goblin")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Goblin")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Goblin")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Goblin")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
                ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().targetCard = false;
                    if(ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy)
                       ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlackSlime" || ObjectSet.Enemy_Name[1] == "BlackSlime" ||
                ObjectSet.Enemy_Name[2] == "BlackSlime" || ObjectSet.Enemy_Name[3] == "BlackSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DeathBringer" || ObjectSet.Enemy_Name[1] == "DeathBringer" ||
                ObjectSet.Enemy_Name[2] == "DeathBringer" || ObjectSet.Enemy_Name[3] == "DeathBringer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWizard" || ObjectSet.Enemy_Name[1] == "FireWizard" ||
                ObjectSet.Enemy_Name[2] == "FireWizard" || ObjectSet.Enemy_Name[3] == "FireWizard")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().targetCard = false;  
                    if (ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Necromancer" || ObjectSet.Enemy_Name[1] == "Necromancer" ||
                ObjectSet.Enemy_Name[2] == "Necromancer" || ObjectSet.Enemy_Name[3] == "Necromancer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWorm" || ObjectSet.Enemy_Name[1] == "FireWorm" ||
                ObjectSet.Enemy_Name[2] == "FireWorm" || ObjectSet.Enemy_Name[3] == "FireWorm")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FlyDemon" || ObjectSet.Enemy_Name[1] == "FlyDemon" ||
                ObjectSet.Enemy_Name[2] == "FlyDemon" || ObjectSet.Enemy_Name[3] == "FlyDemon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "miniMushroom" || ObjectSet.Enemy_Name[1] == "miniMushroom" ||
                ObjectSet.Enemy_Name[2] == "miniMushroom" || ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Bat" || ObjectSet.Enemy_Name[1] == "Bat" ||
                ObjectSet.Enemy_Name[2] == "Bat" || ObjectSet.Enemy_Name[3] == "Bat")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Bat")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Bat")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Bat")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Bat")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "OrangeGolem" || ObjectSet.Enemy_Name[1] == "OrangeGolem" ||
                ObjectSet.Enemy_Name[2] == "OrangeGolem" || ObjectSet.Enemy_Name[3] == "OrangeGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlueGolem" || ObjectSet.Enemy_Name[1] == "BlueGolem" ||
                ObjectSet.Enemy_Name[2] == "BlueGolem" || ObjectSet.Enemy_Name[3] == "BlueGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior01" || ObjectSet.Enemy_Name[1] == "GhostWarrior01" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior01" || ObjectSet.Enemy_Name[3] == "GhostWarrior01")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior02" || ObjectSet.Enemy_Name[1] == "GhostWarrior02" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior02" || ObjectSet.Enemy_Name[3] == "GhostWarrior02")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior03" || ObjectSet.Enemy_Name[1] == "GhostWarrior03" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior03" || ObjectSet.Enemy_Name[3] == "GhostWarrior03")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior04" || ObjectSet.Enemy_Name[1] == "GhostWarrior04" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior04" || ObjectSet.Enemy_Name[3] == "GhostWarrior04")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy = false;
                }
            }

            // BOSS

            if (ObjectSet.Enemy_Name[0] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[1] == "AxeCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[1] == "SpearCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Titan" || ObjectSet.Enemy_Name[1] == "Titan" ||
                ObjectSet.Enemy_Name[2] == "Titan" || ObjectSet.Enemy_Name[3] == "Titan")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Titan")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Titan")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Titan")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Titan")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DemonSlime" || ObjectSet.Enemy_Name[1] == "DemonSlime" ||
                ObjectSet.Enemy_Name[2] == "DemonSlime" || ObjectSet.Enemy_Name[3] == "DemonSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Argon" || ObjectSet.Enemy_Name[1] == "Argon" ||
                ObjectSet.Enemy_Name[2] == "Argon" || ObjectSet.Enemy_Name[3] == "Argon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Argon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Argon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Argon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Argon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().targetCard = false;
                    if (ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy = false;
                }
            }
        }
    }
    void EnemymultipleTarget(bool TF)
    {
        if(TF)
        {
            if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
            ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
                ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Eye")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Eye")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Eye")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Eye")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
                ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Goblin")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Goblin")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Goblin")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Goblin")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
                ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if(!ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlackSlime" || ObjectSet.Enemy_Name[1] == "BlackSlime" ||
                ObjectSet.Enemy_Name[2] == "BlackSlime" || ObjectSet.Enemy_Name[3] == "BlackSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DeathBringer" || ObjectSet.Enemy_Name[1] == "DeathBringer" ||
                ObjectSet.Enemy_Name[2] == "DeathBringer" || ObjectSet.Enemy_Name[3] == "DeathBringer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWizard" || ObjectSet.Enemy_Name[1] == "FireWizard" ||
                ObjectSet.Enemy_Name[2] == "FireWizard" || ObjectSet.Enemy_Name[3] == "FireWizard")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Necromancer" || ObjectSet.Enemy_Name[1] == "Necromancer" ||
                ObjectSet.Enemy_Name[2] == "Necromancer" || ObjectSet.Enemy_Name[3] == "Necromancer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWorm" || ObjectSet.Enemy_Name[1] == "FireWorm" ||
                ObjectSet.Enemy_Name[2] == "FireWorm" || ObjectSet.Enemy_Name[3] == "FireWorm")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FlyDemon" || ObjectSet.Enemy_Name[1] == "FlyDemon" ||
                ObjectSet.Enemy_Name[2] == "FlyDemon" || ObjectSet.Enemy_Name[3] == "FlyDemon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "miniMushroom" || ObjectSet.Enemy_Name[1] == "miniMushroom" ||
            ObjectSet.Enemy_Name[2] == "miniMushroom" || ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Bat" || ObjectSet.Enemy_Name[1] == "Bat" ||
                ObjectSet.Enemy_Name[2] == "Bat" || ObjectSet.Enemy_Name[3] == "Bat")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Bat")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Bat")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Bat")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Bat")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "OrangeGolem" || ObjectSet.Enemy_Name[1] == "OrangeGolem" ||
                ObjectSet.Enemy_Name[2] == "OrangeGolem" || ObjectSet.Enemy_Name[3] == "OrangeGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlueGolem" || ObjectSet.Enemy_Name[1] == "BlueGolem" ||
                ObjectSet.Enemy_Name[2] == "BlueGolem" || ObjectSet.Enemy_Name[3] == "BlueGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior01" || ObjectSet.Enemy_Name[1] == "GhostWarrior01" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior01" || ObjectSet.Enemy_Name[3] == "GhostWarrior01")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior02" || ObjectSet.Enemy_Name[1] == "GhostWarrior02" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior02" || ObjectSet.Enemy_Name[3] == "GhostWarrior02")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior03" || ObjectSet.Enemy_Name[1] == "GhostWarrior03" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior03" || ObjectSet.Enemy_Name[3] == "GhostWarrior03")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior04" || ObjectSet.Enemy_Name[1] == "GhostWarrior04" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior04" || ObjectSet.Enemy_Name[3] == "GhostWarrior04")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().HIT_Enemy = true;
                }
            }
            // BOSS

            if (ObjectSet.Enemy_Name[0] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[1] == "AxeCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[1] == "SpearCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Titan" || ObjectSet.Enemy_Name[1] == "Titan" ||
                ObjectSet.Enemy_Name[2] == "Titan" || ObjectSet.Enemy_Name[3] == "Titan")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Titan")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Titan")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Titan")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Titan")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DemonSlime" || ObjectSet.Enemy_Name[1] == "DemonSlime" ||
                ObjectSet.Enemy_Name[2] == "DemonSlime" || ObjectSet.Enemy_Name[3] == "DemonSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().HIT_Enemy = true;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Argon" || ObjectSet.Enemy_Name[1] == "Argon" ||
                ObjectSet.Enemy_Name[2] == "Argon" || ObjectSet.Enemy_Name[3] == "Argon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Argon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().HIT_Enemy = true;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Argon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Argon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Argon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Arrow = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Guide = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().targetCard = true;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Card_Damage = deckField.Click_Card.multiple_damage;
                    if (!ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy)
                        ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().HIT_Enemy = true;
                }
            }
        }
        else
        {
            if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
            ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Skeleton_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Arrow = false; 
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().Guide = false; 
                    ObjectSet.Field_inMonster[1].GetComponent<Skeleton_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Skeleton_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Skeleton")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Skeleton_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
                ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Eye")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Eye_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Eye")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Eye_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Eye")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Eye_Script>().targetCard = false;

                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Eye")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Eye_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
                ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Goblin")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Goblin_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Goblin")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Goblin_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Goblin")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Goblin_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Goblin")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Goblin_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
                ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Mushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Mushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Mushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Mushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Mushroom_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlackSlime" || ObjectSet.Enemy_Name[1] == "BlackSlime" ||
                ObjectSet.Enemy_Name[2] == "BlackSlime" || ObjectSet.Enemy_Name[3] == "BlackSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlackSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlackSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlackSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlackSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlackSlime_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DeathBringer" || ObjectSet.Enemy_Name[1] == "DeathBringer" ||
                ObjectSet.Enemy_Name[2] == "DeathBringer" || ObjectSet.Enemy_Name[3] == "DeathBringer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DeathBringer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DeathBringer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DeathBringer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DeathBringer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DeathBringer_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWizard" || ObjectSet.Enemy_Name[1] == "FireWizard" ||
                ObjectSet.Enemy_Name[2] == "FireWizard" || ObjectSet.Enemy_Name[3] == "FireWizard")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWizard_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWizard_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWizard_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWizard")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWizard_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Necromancer" || ObjectSet.Enemy_Name[1] == "Necromancer" ||
                ObjectSet.Enemy_Name[2] == "Necromancer" || ObjectSet.Enemy_Name[3] == "Necromancer")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Necromancer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Necromancer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Necromancer_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Necromancer")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Necromancer_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FireWorm" || ObjectSet.Enemy_Name[1] == "FireWorm" ||
                ObjectSet.Enemy_Name[2] == "FireWorm" || ObjectSet.Enemy_Name[3] == "FireWorm")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FireWorm_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FireWorm_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FireWorm_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FireWorm")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FireWorm_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "FlyDemon" || ObjectSet.Enemy_Name[1] == "FlyDemon" ||
                ObjectSet.Enemy_Name[2] == "FlyDemon" || ObjectSet.Enemy_Name[3] == "FlyDemon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<FlyDemon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<FlyDemon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<FlyDemon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "FlyDemon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<FlyDemon_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "miniMushroom" || ObjectSet.Enemy_Name[1] == "miniMushroom" ||
                ObjectSet.Enemy_Name[2] == "miniMushroom" || ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<miniMushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<miniMushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<miniMushroom_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<miniMushroom_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Bat" || ObjectSet.Enemy_Name[1] == "Bat" ||
                ObjectSet.Enemy_Name[2] == "Bat" || ObjectSet.Enemy_Name[3] == "Bat")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Bat")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Bat_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Bat")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Bat_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Bat")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Bat_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Bat")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Bat_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "OrangeGolem" || ObjectSet.Enemy_Name[1] == "OrangeGolem" ||
                ObjectSet.Enemy_Name[2] == "OrangeGolem" || ObjectSet.Enemy_Name[3] == "OrangeGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<OrangeGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<OrangeGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<OrangeGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "OrangeGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<OrangeGolem_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "BlueGolem" || ObjectSet.Enemy_Name[1] == "BlueGolem" ||
                ObjectSet.Enemy_Name[2] == "BlueGolem" || ObjectSet.Enemy_Name[3] == "BlueGolem")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<BlueGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<BlueGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<BlueGolem_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "BlueGolem")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<BlueGolem_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior01" || ObjectSet.Enemy_Name[1] == "GhostWarrior01" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior01" || ObjectSet.Enemy_Name[3] == "GhostWarrior01")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior01_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior01_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior01_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior01")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior01_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior02" || ObjectSet.Enemy_Name[1] == "GhostWarrior02" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior02" || ObjectSet.Enemy_Name[3] == "GhostWarrior02")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior02_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior02_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior02_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior02")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior02_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior03" || ObjectSet.Enemy_Name[1] == "GhostWarrior03" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior03" || ObjectSet.Enemy_Name[3] == "GhostWarrior03")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior03_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior03_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior03_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior03")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior03_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "GhostWarrior04" || ObjectSet.Enemy_Name[1] == "GhostWarrior04" ||
                ObjectSet.Enemy_Name[2] == "GhostWarrior04" || ObjectSet.Enemy_Name[3] == "GhostWarrior04")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<GhostWarrior04_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<GhostWarrior04_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<GhostWarrior04_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "GhostWarrior04")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<GhostWarrior04_Script>().targetCard = false;
                }
            }
            // BOSS

            if (ObjectSet.Enemy_Name[0] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[1] == "AxeCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "AxeCastleGuardian" || ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "AxeCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<AxeCastleGuardian_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[1] == "SpearCastleGuardian" ||
                ObjectSet.Enemy_Name[2] == "SpearCastleGuardian" || ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "SpearCastleGuardian")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<SpearCastleGuardian_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Titan" || ObjectSet.Enemy_Name[1] == "Titan" ||
                ObjectSet.Enemy_Name[2] == "Titan" || ObjectSet.Enemy_Name[3] == "Titan")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Titan")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Titan_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Titan")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Titan_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Titan")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Titan_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Titan")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Titan_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "DemonSlime" || ObjectSet.Enemy_Name[1] == "DemonSlime" ||
                ObjectSet.Enemy_Name[2] == "DemonSlime" || ObjectSet.Enemy_Name[3] == "DemonSlime")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<DemonSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<DemonSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<DemonSlime_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "DemonSlime")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<DemonSlime_Script>().targetCard = false;
                }
            }
            if (ObjectSet.Enemy_Name[0] == "Argon" || ObjectSet.Enemy_Name[1] == "Argon" ||
                ObjectSet.Enemy_Name[2] == "Argon" || ObjectSet.Enemy_Name[3] == "Argon")
            {
                if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Enemy_Name[0] == "Argon")
                {
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[0].GetComponent<Argon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[1] != null && ObjectSet.Enemy_Name[1] == "Argon")
                {
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[1].GetComponent<Argon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[2] != null && ObjectSet.Enemy_Name[2] == "Argon")
                {
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[2].GetComponent<Argon_Script>().targetCard = false;
                }
                if (ObjectSet.Field_inMonster[3] != null && ObjectSet.Enemy_Name[3] == "Argon")
                {
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Arrow = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().Guide = false;
                    ObjectSet.Field_inMonster[3].GetComponent<Argon_Script>().targetCard = false;
                }
            }
        }
    }

    void Object_inName() // 카드를 들고 마우스커서를 캐릭터에 가져갔을때 해당 이름은 무엇인가?
    {
        switch (Object_name)
        {
            case "Player": // 플레이어 일 경우
                Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 10, 0);
                transform.position = Vector3.Lerp(transform.position, playerPosition, 5 * Time.deltaTime);
                break;

            case "Skeleton": // 적 캐릭터 일 경우
            case "Eye":
            case "Goblin":
            case "Mushroom":
            case "BlackSlime":
            case "DeathBringer":
            case "FireWizard":
            case "Necromancer":
            case "FireWorm":
            case "FlyDemon":
            case "miniMushroom":
            case "Bat":
            case "OrangeGolem":
            case "BlueGolem":
            case "GhostWarrior01":
            case "GhostWarrior02":
            case "GhostWarrior03":
            case "GhostWarrior04":
            case "Argon":
                switch (Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                        Vector3 SinglePositon = new Vector3(Card_transform.x, Card_transform.y + 6.5f, 0);
                        transform.position = Vector3.Lerp(transform.position, SinglePositon, 5 * Time.deltaTime);

                        break;

                    case "화염장판":
                    case "얼음안개":
                        if(Card_upNumber == 0)
                        {
                            Vector3 MultiplePositon = new Vector3(Card_transform.x + 10.5f, Card_transform.y + 7f, 0);
                            transform.position = Vector3.Lerp(transform.position, MultiplePositon, 5 * Time.deltaTime);
                        }
                        if(Card_upNumber == 1)
                        {
                            Vector3 MultiplePositon = new Vector3(Card_transform.x + 3.5f, Card_transform.y + 7f, 0);
                            transform.position = Vector3.Lerp(transform.position, MultiplePositon, 5 * Time.deltaTime);
                        }
                        if(Card_upNumber == 2)
                        {
                            Vector3 MultiplePositon = new Vector3(Card_transform.x - 3.5f, Card_transform.y + 7f, 0);
                            transform.position = Vector3.Lerp(transform.position, MultiplePositon, 5 * Time.deltaTime);
                        }
                        if(Card_upNumber == 3)
                        {
                            Vector3 MultiplePositon = new Vector3(Card_transform.x - 10.5f, Card_transform.y + 7f, 0);
                            transform.position = Vector3.Lerp(transform.position, MultiplePositon, 5 * Time.deltaTime);
                        }
                        break;
                    default:
                        break;
                }
                break;

            case "AxeCastleGuardian":
            case "SpearCastleGuardian":
            case "Titan":
            case "DemonSlime":
                switch(Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                    case "화염장판":
                    case "얼음안개":
                        Vector3 SinglePositon = new Vector3(Card_transform.x - 8f, Card_transform.y, 0);
                        transform.position = Vector3.Lerp(transform.position, SinglePositon, 5 * Time.deltaTime);
                        break;
                    default:
                        break;
                }
                break;
            
            default: // 그외
                Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                transform.position = Vector3.Lerp(transform.position, mousePosition, 10 * Time.deltaTime);
                break;
        }
    }
}
