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

    public int Card_Number = 0;

    public string Card_name;
    public int single_damage;
    public int multiple_damage;
    public int health;
    public int mana;
    public int count;

    public int Card_status;
    public string Object_name;

    public float Mouse_insideCard_timer = 0f;
    float Card_timer = 0f;

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
        if (Card_timer < 2f) Card_timer += Time.deltaTime;
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
        if(Card_timer > 2f) CardScale(true); // 1초 이상 해당 오브젝트에 마우스 커서가 있을 경우
    }
    private void OnMouseExit() // 마우스 커서가 해당 오브젝트의 BoxCollider2D 밖으로 나갔는지 확인하는 이벤트
    {
        CardScale(false); // 크기 줄이기
        Mouse_insideCard_timer = 0f; // 타이머 초기화
    }
    private void OnMouseDown() // 마우스 커서가 해당 오브젝트의 BoxCollider2D 안에서 왼 클릭을 했는지 확인하는 이벤트
    {
        if(!Card_MouseClick)
        {
            Card_MouseClick = true;         // 카드 선택
            Card_noHide = true;             // 선택한 카드는 숨기지 못하게 막기
            deckField.cardMove_Rock = true; // 카드 움직임 멈추기
            deckField.cardHide = true;      // 카드 숨기기
            Target_Card(Card_MouseClick);
            deckField.Click_Card = this.gameObject.GetComponent<Card_Script>();
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
                case "하급회복포션":
                    player.targetPlayerCard = true;
                    break;
                case "상급회복포션":
                    player.targetPlayerCard = true;
                    break;
                case "명상":
                    player.targetPlayerCard = true;
                    break;
                case "일반마법":
                    EnemyTarget();
                    break;
                case "화염장판":
                    EnemyTarget();
                    break;
                case "얼음안개":
                    EnemyTarget();
                    break;
                case "바람의창":
                    EnemyTarget();
                    break;
                case "돌무더기":
                    EnemyTarget();
                    break;
                case "생명의잔불":
                    player.targetPlayerCard = true;
                    break;
                case "고요한안식":
                    player.targetPlayerCard = true;
                    break;
                case "절망의균열":
                    EnemyTarget();
                    break;
                case "잔혹한계약":
                    player.targetPlayerCard = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (player.targetPlayerCard) player.targetPlayerCard = false;

            if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
            ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
            {
                if (skeleton.targetCard) skeleton.targetCard = false;
            }
            if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
                ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
            {
                if (eye.targetCard) eye.targetCard = false;
            }
            if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
                ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
            {
                if (goblin.targetCard) goblin.targetCard = false;
            }
            if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
                ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
            {
                if (mushroom.targetCard) mushroom.targetCard = false;
            }
        }

    }
    void EnemyTarget()
    {
        if (ObjectSet.Enemy_Name[0] == "Skeleton" || ObjectSet.Enemy_Name[1] == "Skeleton" ||
            ObjectSet.Enemy_Name[2] == "Skeleton" || ObjectSet.Enemy_Name[3] == "Skeleton")
        {
            skeleton.targetCard = true;
            if (skeleton.HIT_Enemy) skeleton.HIT_Enemy = false;
        }
        if (ObjectSet.Enemy_Name[0] == "Eye" || ObjectSet.Enemy_Name[1] == "Eye" ||
            ObjectSet.Enemy_Name[2] == "Eye" || ObjectSet.Enemy_Name[3] == "Eye")
        {
            eye.targetCard = true;
            if (eye.HIT_Enemy) eye.HIT_Enemy = false;
        }
        if (ObjectSet.Enemy_Name[0] == "Goblin" || ObjectSet.Enemy_Name[1] == "Goblin" ||
            ObjectSet.Enemy_Name[2] == "Goblin" || ObjectSet.Enemy_Name[3] == "Goblin")
        {
            goblin.targetCard = true;
            if (goblin.HIT_Enemy) goblin.HIT_Enemy = false;
        }
        if (ObjectSet.Enemy_Name[0] == "Mushroom" || ObjectSet.Enemy_Name[1] == "Mushroom" ||
            ObjectSet.Enemy_Name[2] == "Mushroom" || ObjectSet.Enemy_Name[3] == "Mushroom")
        {
            mushroom.targetCard = true;
            if (mushroom.HIT_Enemy) mushroom.HIT_Enemy = false;
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
            case "Skeleton": // 스켈레톤 일 경우
                Vector3 skeletonPositon = new Vector3(skeleton.transform.position.x, skeleton.transform.position.y + 7, 0);
                transform.position = Vector3.Lerp(transform.position, skeletonPositon, 5 * Time.deltaTime);
                break;
            case "Eye":
                Vector3 eyePosition = new Vector3(eye.transform.position.x, eye.transform.position.y + 7, 0);
                transform.position = Vector3.Lerp(transform.position, eyePosition, 5 * Time.deltaTime);
                break;
            case "Goblin":
                Vector3 GoblinPosition = new Vector3(goblin.transform.position.x, goblin.transform.position.y + 7, 0);
                transform.position = Vector3.Lerp(transform.position, GoblinPosition, 5 * Time.deltaTime);
                break;
            case "Mushroom":
                Vector3 MushroomPosition = new Vector3(mushroom.transform.position.x, mushroom.transform.position.y + 7, 0);
                transform.position = Vector3.Lerp(transform.position, MushroomPosition, 5 * Time.deltaTime);
                break;
            default: // 그외
                Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                transform.position = Vector3.Lerp(transform.position, mousePosition, 10 * Time.deltaTime);
                break;
        }
    }
}
