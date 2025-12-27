using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Script : MonoBehaviour
{
    CardDeckField_Script deckField;
    CardDeck_Script deck;
    Card_Script card;
    Animator animator;

    public GameObject PlayerFunnel_Image;
    GameObject PlayerFunnel;

    public Charater_namedata unitname;
    public Charater_Status status;

    public int nowHp;
    public int maxHp;
    public int nowMp;
    public int maxMp;

    public int Hit_percent = 3;     // 해당값을 추후 아이템으로 변경됨
    public int Defence_percent = 1; // <-
    public int Avoid_percent = 0;   // <-

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

    public float PlayerAttack_timer = 0f;
    public float PlayerDead_timer = 0f;

    public Vector3 baseTransform;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        baseTransform = transform.position;

        deckField = FindObjectOfType<CardDeckField_Script>();
        deck = FindObjectOfType<CardDeck_Script>();

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

        if (PlayerFunnel != null && !targetPlayerCard) Destroy(PlayerFunnel);
        if(PlayerFunnel == null)
        {
            if (targetPlayerCard) // 카드를 클릭했는지 감지 -> 대상으로 변경 할 예정
            {
                Vector3 player_offset = new Vector3(0, 6, 0); // 플레이어의 offset 값에 + 6을 더함
                PlayerFunnel = Instantiate(PlayerFunnel_Image, player_offset, Quaternion.identity); // 해당 오브젝트의 복제본을 생성
                Funnel_Script funnel = PlayerFunnel.GetComponent<Funnel_Script>(); // 복제본에 스크립트파일 대입하기
                funnel.target = this.transform; // 복제된 오브젝트의 위치
                funnel.offset = player_offset; // 오브젝트에 더할값
            }
        }
        if(animation_Attack)
        {
            if(animator.GetBool("PlayerIdle"))
            {
                animator.SetBool("PlayerIdle", false);
                animator.SetBool("PlayerMove", true);
            }
            if(animator.GetBool("PlayerMove"))
            {
                if (transform.position.x < -5)
                { transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, transform.position.y, 0); }
                else
                { 
                    animator.SetBool("PlayerMove", false);
                    animator.SetBool("PlayerAttack", true);
                    PlayerAttack_Enemy = true; // !+ 적캐릭터 피격 활성화
                }
            }
            if(animator.GetBool("PlayerAttack"))
            {
                if(PlayerAttack_timer < 1f)
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
            if(animator.GetBool("PlayerBackMove"))
            {
                if(transform.position.x > -10)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.position = new Vector3(transform.position.x - 15f * Time.deltaTime, transform.position.y, 0); 
                }
                else
                {
                    animator.SetBool("PlayerBackMove", false);
                    animator.SetBool("PlayerIdle", true);
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.position = new Vector3(-10, -1, 0);
                    animation_Attack = false;
                }
            }
        }
    }

    private void OnMouseOver() // 마우스커서가 오브젝트에 있는지 감지하는 이벤트
    {
        Player_cardUse = true;
        if (targetPlayerCard)
        {
            Card_ScriptCheck(true);
            card.Object_name = "Player";
        }
    }
    private void OnMouseExit() // 마우스커서가 오브젝트 없는지 감지하는 이벤트
    {
        Player_cardUse = false;
        if (targetPlayerCard)
        {
            card.Object_name = "";
        }
    }
    private void OnMouseDown() // 마우스커서가 오브젝트에서 클릭을 했는지 감지하는 이벤트
    {
        if (Player_cardUse)
        {
            card.CardDestroy();
            card = null;
            deck.CardCount--;
            deckField.cardHide = false;
            targetPlayerCard = false;
        }
    }
    void Card_ScriptCheck(bool isinside) // 클릭한 카드 찾기
    {
        if(isinside)
        {
            for(int i = 0; i < deckField.Card_inField.Length; i++)
            {
                card = deckField.Card_inField_Script[i]; // 스크립트 대입
                if (card.Card_MouseClick) break; // 대입한 스크립트에 마우스클릭이 켜져있으면 정지
            }
        }
    }

    void HIT_percent()
    {
        if (EnemyAttack_Player) // <- 해당 불린을 1초간 지속이기 때문이 밑의 코드가 100번 이상은 실행된다
        {
            if (Percent == -1) Percent = Random.Range(0, 11); // <- 공격 확률은 기본 회피 10% : 막기 20% : 맞기 70% 

            if (Hit_percent <= Percent) // 기본값 3보다 크거나 같을 경우 3~10 : 70%
            {
                if (EnemyAttack_Player) animator.SetBool("PlayerHit", true);
                PlayerDamage = true;
                Full_hit = true;
                
            }
            if (Defence_percent <= Percent && Percent < Hit_percent) // 기본값 1보다 크거나 같을경우 그리고 3보다 작을경우 1~2 : 20%
            {
                if (EnemyAttack_Player) animator.SetBool("PlayerDefence", true);
                PlayerDamage = true;
                Half_hit = true;
            }
            if (Avoid_percent <= Percent && Percent < Defence_percent) // 기본값 0보다 크거나 같을 경우 그리고 1보다 작을경우 0 : 10%
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
            if(Avoid)
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (transform.position.x <= -10)
                {
                    transform.position = new Vector3(transform.position.x + 15 * Time.deltaTime, transform.position.y, 0);
                }
                else
                {
                    transform.position = new Vector3(-10, -1, 0);
                    if (animator.GetBool("PlayerAvoid")) animator.SetBool("PlayerAvoid", false);
                    Avoid = false;
                }
            }
            if (PlayerDamage)
            {
                if(Full_hit) { nowHp -= 10; Full_hit = false; } // 전체 데미지
                if(Half_hit) {  nowHp -= 5; Half_hit = false; } // 전체 데미지의 반절
                if (nowHp <= 0f) animator.SetTrigger("PlayerDie");
                PlayerDamage = false;
            }
        }
    }
}
