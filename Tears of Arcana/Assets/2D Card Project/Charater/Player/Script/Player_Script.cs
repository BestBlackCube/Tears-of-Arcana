using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Script : MonoBehaviour
{
    public EnemyObjectSet_Script ObjectSet;
    public CardDeckField_Script deckField;
    public CardDeck_Script deck;
    Card_Script card;
    Animator animator;

    public GameObject BlackScreen_prefab;

    public GameObject PlayerTarget_prefab;
    GameObject targetGuide;
    public GameObject TargetArrow_prefab;
    GameObject targetArrow;

    public bool Arrow = false;

    public Charater_namedata unitname;
    public Charater_Status status;

    public int nowHp;
    public int maxHp;
    public int nowMp;
    public int maxMp;
    public int HitDamage;

    public int Hit_percent = 30;     // 해당값을 추후 아이템으로 변경됨
    public int Defence_percent = 10; // <-
    public int Avoid_percent = 0;   // <-

    public int item_Top;
    public int item_Bottom;

    float Percent = -1; // -1을 하는이유는 해당 값이 1초동안 1~10의 난수가 100번 이상 반복되기때문에 100% 확률성으로 볼수 없어 초기값을 -1 지정

    public bool PlayerDamage = false;
    public bool targetPlayerCard = false;
    public bool animation_Attack = false;
    public bool PlayerAttack_Enemy = false;
    public bool EnemyAttack_Player = false;
    
    bool Player_cardUse = false;
    bool Avoid = false;
    bool Full_hit = false;
    bool Half_hit = false;
    bool Move = false;
    bool Screen_On = false;

    int MoveCount = 0;

    public float PlayerAttack_timer = 0f;
    public float PlayerDead_timer = 0f;
    public float Delay_timer = 0f;

    public Vector3 baseTransform;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        baseTransform = transform.position;


        status = new Charater_Status();
        status = status.Char_inStatus(unitname);

        nowHp = status.NowHp;
        maxHp = status.MaxHp;

        nowMp = status.NowMp;
        maxMp = status.MaxMp;
    }

    // Update is called once per frame
    void Update()
    {
        HIT_percent();
        if (animation_Attack) Player_AttackStart();
        if (!Arrow && targetArrow != null) Destroy(targetArrow);
        if (targetPlayerCard && Arrow && targetArrow == null)
        {
            Vector3 player_offset = new Vector3(transform.position.x, 6, 0);
            targetArrow = Instantiate(TargetArrow_prefab, player_offset, Quaternion.identity);
            TargetArrow_Script arrow = targetArrow.GetComponent<TargetArrow_Script>();
            arrow.offset = player_offset;
        }
    }
    void FixedUpdate()
    {
        if(Move)
        {
            if(MoveCount == 1)
            {
                PlayerMove(25);
                animator.SetBool("PlayerIdle", false);
                animator.SetBool("PlayerMove", true);
                if(transform.position.x > 25)
                {
                    if(Screen_On)
                    {
                        BlackScreen_prefab.SetActive(true);
                        BlackScreen_prefab.GetComponent<BlackScreen_Script>().PlayerNext = true;
                        Screen_On = false;
                    }
                    if (Delay_timer < 8) Delay_timer += Time.deltaTime;
                    else
                    {
                        transform.position = new Vector3(-25, -1, 5);
                        MoveCount++;
                    }
                }
            }
            if(MoveCount == 2)
            {
                PlayerMove(-15);
                if (transform.position.x > -15) MoveCount++;
            }
            if(MoveCount == 3)
            {
                animator.SetBool("PlayerIdle", true);
                animator.SetBool("PlayerMove", false);
                MoveCount = 0;
                Move = false;
            }
        }
    }
    void PlayerMove(int Range)
    {
        if (transform.position.x < Range) transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, -1, 5);
    }
    private void OnMouseOver() // 마우스커서가 오브젝트에 있는지 감지하는 이벤트
    {
        Player_cardUse = true;
        if (Arrow && targetArrow != null) Arrow = false;
        if (targetPlayerCard) deckField.Click_Card.Object_name = "Player";

        if (targetPlayerCard && targetGuide == null)
        {
            Vector3 player_offset = new Vector3(transform.position.x, 6, 0);
            targetGuide = Instantiate(PlayerTarget_prefab, player_offset, Quaternion.identity); // 해당 오브젝트의 복제본을 생성
            EnemyTargetBar_Script guide = targetGuide.GetComponent<EnemyTargetBar_Script>(); // 복제본에 스크립트파일 대입하기
            guide.target = this.transform; // 복제된 오브젝트의 위치
            guide.offset[0] = new Vector3(-1.3f, 5f, 0);
            guide.offset[1] = new Vector3(0.7f, 5f, 0);
            guide.offset[2] = new Vector3(-1.3f, 0f, 0);
            guide.offset[3] = new Vector3(0.7f, 0f, 0);
        }

    }
    private void OnMouseExit() // 마우스커서가 오브젝트 없는지 감지하는 이벤트
    {
        Player_cardUse = false;
        if (targetPlayerCard) deckField.Click_Card.Object_name = "";
        if (!Arrow && targetArrow == null) Arrow = true;
        if (targetGuide != null) Destroy(targetGuide);
    }
    private void OnMouseDown() // 마우스커서가 오브젝트에서 클릭을 했는지 감지하는 이벤트
    {
        if(deckField.Click_Card != null)
        if (Player_cardUse && deckField.Click_Card.Object_name == "Player")
        {
            deckField.Click_Card.Card_MouseClick = false;
            deckField.Click_Card.Target_Card(false);
            CardName_inStatus(deckField.Click_Card.Card_name);
            deckField.Click_Card.CardDestroy();
            deckField.Click_Card = null;
            deckField.cardHide = false;
            targetPlayerCard = false;
        }
    }
    void HIT_percent()
    {
        if (EnemyAttack_Player) // <- 해당 불린을 1초간 지속이기 때문이 밑의 코드가 100번 이상은 실행된다
        {
            if (Percent == -1) Percent = Random.Range(0, 101); // <- 공격 확률은 기본 회피 10% : 막기 20% : 맞기 70% 

            if (Hit_percent <= Percent) // 확률이 30보다 크거나 같을 경우 3~10 : 70%
            {
                if (EnemyAttack_Player) animator.SetBool("PlayerHit", true);
                PlayerDamage = true;
                Full_hit = true;

            }
            if (Defence_percent <= Percent && Percent < Hit_percent) // 기본값 10보다 크거나 같을경우 그리고 30보다 작을경우 10~20 : 20%
            {
                if (EnemyAttack_Player) animator.SetBool("PlayerDefence", true);
                PlayerDamage = true;
                Half_hit = true;
            }
            if (Avoid_percent <= Percent && Percent < Defence_percent) // 기본값 0보다 크거나 같을 경우 그리고 10보다 작을경우 0 : 10%
            {

                if (EnemyAttack_Player) animator.SetBool("PlayerAvoid", true);
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = new Vector3(transform.position.x - 15 * Time.deltaTime, transform.position.y, 0);
                Avoid = true;
            }
        }
        else
        {
            Percent = -1;
            if (animator.GetBool("PlayerHit")) animator.SetBool("PlayerHit", false);
            if (animator.GetBool("PlayerDefence")) animator.SetBool("PlayerDefence", false);
            if (Avoid)
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (transform.position.x <= -15)
                {
                    transform.position = new Vector3(transform.position.x + 15 * Time.deltaTime, transform.position.y, 0);
                }
                else
                {
                    transform.position = new Vector3(-15, -1, 0);
                    if (animator.GetBool("PlayerAvoid")) animator.SetBool("PlayerAvoid", false);
                    Avoid = false;
                }
            }
            if (PlayerDamage)
            {
                if (Full_hit) { nowHp -= HitDamage; Full_hit = false; } // 전체 데미지
                if (Half_hit) { nowHp -= HitDamage; Half_hit = false; } // 전체 데미지의 반절
                if (nowHp <= 0f) animator.SetTrigger("PlayerDie");
                HitDamage = 0;
                PlayerDamage = false;
            }
        }
    }
    void Player_AttackStart()
    {
        if (animator.GetBool("PlayerIdle"))
        {
            animator.SetBool("PlayerIdle", false);
            animator.SetBool("PlayerMove", true);
        }
        if (animator.GetBool("PlayerMove"))
        {
            if (transform.position.x < -10)
            { transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, transform.position.y, 0); }
            else
            {
                animator.SetBool("PlayerMove", false);
                animator.SetBool("PlayerAttack", true);
                PlayerAttack_Enemy = true; // !+ 적캐릭터 피격 활성화
            }
        }
        if (animator.GetBool("PlayerAttack"))
        {
            if (PlayerAttack_timer < 1f)
            {
                PlayerAttack_timer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("PlayerAttack", false);
                animator.SetBool("PlayerBackMove", true);
                PlayerAttack_Enemy = false; // !+ 적캐릭터 패격 비활성화
                PlayerAttack_timer = 0f;
            }
        }
        if (animator.GetBool("PlayerBackMove"))
        {
            if (transform.position.x > -15)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = new Vector3(transform.position.x - 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("PlayerBackMove", false);
                animator.SetBool("PlayerIdle", true);
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = new Vector3(-15, -1, 0);
                animation_Attack = false;
            }
        }
    }
    void CardName_inStatus(string name)
    {
        switch(name)
        {
            case "다음으로":
                Screen_On = true;
                Move = true;
                MoveCount++;
                break;
            case "이전으로":
                break;
            case "하급회복물약":
                nowHp += deckField.Click_Card.health;
                if (nowHp > 100) nowHp = 100;
                break;
            case "상급회복물약":
                nowHp += deckField.Click_Card.health;
                if (nowHp > 100) nowHp = 100;
                break;
            case "명상":
                nowHp += deckField.Click_Card.health;
                nowMp += deckField.Click_Card.mana;
                if (nowHp > 100) nowHp = 100;
                break;
            case "생명의잔불":
                nowHp += deckField.Click_Card.health;
                if (nowHp <= 0f) animator.SetTrigger("PlayerDie");
                //deckField.Click_Card.count;
                break;
            case "고요한안식":
                nowHp += deckField.Click_Card.health;
                if (nowHp > 100) nowHp = 100;
                break;
            case "잔혹한계약":
                nowHp += deckField.Click_Card.health;
                break;

            default:
                break;
        }
    }
}
