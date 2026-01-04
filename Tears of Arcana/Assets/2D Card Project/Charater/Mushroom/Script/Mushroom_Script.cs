using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mushroom_Script : MonoBehaviour
{
    public GameObject Funnel_Image;
    GameObject Funnel;

    public GameObject HpBar_prefab;
    public GameObject canvas;

    public Charater_namedata unitname;
    public Charater_Status status;

    RectTransform hpbar;

    Animator animator;

    public int Card_Damage;
    public int nowHp;
    public int maxHp;
    public float Dead_timer = 0f;
    public float Attack_timer = 0f;

    EnemyObjectSet_Script ObjectSet;
    ObjectSet_Script attack_order;
    Player_Script player;
    CardDeckField_Script deckField;
    CardDeck_Script deck;
    public Card_Script card;
    Image nowHpbar;

    public bool targetCard = false;    // 카드가 Eye를 선택하는지
    public bool EnemyDamage = false;        // Eye가 데미지를 받았을때
    public bool animation_Attack = false;
    public bool EnemyAttack = false;
    bool cardUse = false;
    public bool HIT_Enemy = false;

    public Vector3 animation_position;

    // Start is called before the first frame update
    void Start()
    {
        status = new Charater_Status();
        status = status.Char_inStatus(unitname);

        nowHp = status.NowHp;
        maxHp = status.MaxHp;
        canvas = GameObject.Find("HPCanvas");

        ObjectSet = FindObjectOfType<EnemyObjectSet_Script>();
        attack_order = FindObjectOfType<ObjectSet_Script>();

        hpbar = Instantiate(HpBar_prefab, canvas.transform).GetComponent<RectTransform>();
        animationPosition();

        nowHpbar = hpbar.transform.GetChild(0).GetComponent<Image>();
        animator = GetComponent<Animator>();

        player = FindObjectOfType<Player_Script>();
        deckField = FindObjectOfType<CardDeckField_Script>();
        deck = FindObjectOfType<CardDeck_Script>();

    }

    // Update is called once per frame
    void Update()
    {
        Attack_Order();
        if (EnemyAttack) Attack();
        if (HIT_Enemy) Hit_Enemy();

        if (Funnel == null && targetCard) // 깔때기 생성
        {
            Vector3 offset = new Vector3(0, 3.5f, 0);
            Funnel = Instantiate(Funnel_Image, offset, Quaternion.identity);
            Funnel_Script funnel = Funnel.GetComponent<Funnel_Script>();
            funnel.target = this.transform;
            funnel.offset = offset;
        }
        if (Funnel != null && !targetCard)
        {
            Destroy(Funnel); // 깔때기 제거
        }
    }
    void OnMouseOver()
    {
        cardUse = true;
        if (targetCard && deckField.Click_Card != null) deckField.Click_Card.Object_name = "Mushroom";
    }
    void OnMouseExit()
    {
        cardUse = false;
        if (targetCard && deckField.Click_Card != null) deckField.Click_Card.Object_name = "";
    }
    void OnMouseDown()
    {
        if (cardUse && deckField.Click_Card.Object_name == "Mushroom")
        {
            deckField.Click_Card.Card_MouseClick = false;
            deckField.Click_Card.Target_Card(false);
            CardData_inEnemy(deckField.Click_Card.Card_name);
            deckField.Click_Card.CardDestroy();
            deckField.Click_Card = null;
            deck.CardCount = 0;
            deckField.cardHide = false;
            player.animation_Attack = true;
            player.targetPlayerCard = false;
            targetCard = false;
            HIT_Enemy = true;
        }
    }
    void Attack() // 공격 애니메이션 코드
    {
        if (animator.GetBool("Idle"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Move", true);
        }
        if (animator.GetBool("Move"))
        {
            if (transform.position.x > -10)
            {
                transform.position = new Vector3(transform.position.x - 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("Move", false);
                animator.SetBool("Attack", true);
                player.EnemyAttack_Player = true; // !+ 플레이어 피격 애니메이션 활성화
            }
        }
        if (animator.GetBool("Attack"))
        {
            if (Attack_timer < 1.1f)
            {
                Attack_timer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("Attack", false);
                animator.SetBool("BackMove", true);
                player.EnemyAttack_Player = false; // !+ 플레이어 피격 애니메이션 비활성화
                player.PlayerDamage = true;        // 플레이어HP 줄이기
                Attack_timer = 0f;
            }
        }
        if (animator.GetBool("BackMove"))
        {
            if (transform.position.x < animation_position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("BackMove", false);
                animator.SetBool("Idle", true);
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = animation_position;
                animation_Attack = true;
            }
        }
    }
    void Hit_Enemy() // 플레이어에게 공격 받았을때 실행 되는 애니메이션
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        if (player.PlayerAttack_Enemy)
        {
            animator.SetBool("Hit", true);
            if (player.PlayerAttack_timer > 1f) EnemyDamage = true;
        }
        else
        {
            animator.SetBool("Hit", false);
        }

        if (EnemyDamage)
        {
            nowHp -= Card_Damage;
            Card_Damage = 0;
            EnemyDamage = false;
        }

        if (nowHp <= 0f)
        {
            animator.SetTrigger("Die");
            if (Dead_timer < 0.6f) Dead_timer += Time.deltaTime;
            else
            {
                Enemy_NameLess();
                Destroy(gameObject);
                Destroy(hpbar.gameObject);
            }
        }
    }
    void animationPosition()
    {
        if (ObjectSet.Enemy_Name[0] == "Mushroom")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[0];
        }
        if (ObjectSet.Enemy_Name[1] == "Mushroom")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[1];
        }
        if (ObjectSet.Enemy_Name[2] == "Mushroom")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[2];
        }
        if (ObjectSet.Enemy_Name[3] == "Mushroom")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[3];
        }
    }
    void Attack_Order()
    {
        if (ObjectSet.Enemy_Name[0] == "Mushroom" && attack_order.Order_1)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_1 = false;
                if (ObjectSet.Enemy_Name[1] != null) attack_order.Order_2 = true;
                else if (ObjectSet.Enemy_Name[2] != null) attack_order.Order_3 = true;
                else if (ObjectSet.Enemy_Name[3] != null) attack_order.Order_4 = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
        if (ObjectSet.Enemy_Name[1] == "Mushroom" && attack_order.Order_2)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_2 = false;
                if (ObjectSet.Enemy_Name[2] != null) attack_order.Order_3 = true;
                else if (ObjectSet.Enemy_Name[3] != null) attack_order.Order_4 = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
        if (ObjectSet.Enemy_Name[2] == "Mushroom" && attack_order.Order_3)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_3 = false;
                attack_order.Order_4 = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
        if (ObjectSet.Enemy_Name[3] == "Mushroom" && attack_order.Order_4)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_4 = false;
                attack_order.CardAdd = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
    }
    void Enemy_NameLess()
    {
        if (ObjectSet.Enemy_Name[0] == "Mushroom") ObjectSet.Enemy_Name[0] = null;
        if (ObjectSet.Enemy_Name[1] == "Mushroom") ObjectSet.Enemy_Name[1] = null;
        if (ObjectSet.Enemy_Name[2] == "Mushroom") ObjectSet.Enemy_Name[2] = null;
        if (ObjectSet.Enemy_Name[3] == "Mushroom") ObjectSet.Enemy_Name[3] = null;
    }
    void CardData_inEnemy(string name)
    {
        switch (name)
        {
            case "일반마법":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "화염장판":
                Card_Damage = deckField.Click_Card.multiple_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "얼음안개":
                Card_Damage = deckField.Click_Card.multiple_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "바람의창":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "돌무더기":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "절망의균열":
                //Card_Damage = deckField.Click_Card.count;
                //player.nowMp += deckField.Click_Card.mana;
                break;

            default:
                break;
        }
    }
}
