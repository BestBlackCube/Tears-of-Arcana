using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goblin_Script : MonoBehaviour
{
    public GameObject GoblinFunnel_Image;
    GameObject GoblinFunnel;

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

    public bool targetGoblinCard = false;
    public bool GoblinDamage = false;
    public bool animation_Attack = false;
    public bool EnemyAttack = false;
    bool Goblin_cardUse = false;
    public bool HIT_Goblin = false;

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
        if (EnemyAttack) Goblin_Attack();
        if (HIT_Goblin) Hit_Goblin();

        if(GoblinFunnel == null && targetGoblinCard)
        {
            Vector3 Goblin_offset = new Vector3(0, 3.5f, 0);
            GoblinFunnel = Instantiate(GoblinFunnel_Image, Goblin_offset, Quaternion.identity);
            Funnel_Script funnel = GoblinFunnel.GetComponent<Funnel_Script>();
            funnel.target = this.transform;
            funnel.offset = Goblin_offset;
        }
        if(GoblinFunnel == null && !targetGoblinCard)
        {
            Destroy(GoblinFunnel);
        }
    }
    void OnMouseOver()
    {
        Goblin_cardUse = true;
        if (targetGoblinCard && deckField.Click_Card != null) deckField.Click_Card.Object_name = "Goblin";
    }
    void OnMouseExit()
    {
        Goblin_cardUse = false;
        if (targetGoblinCard && deckField.Click_Card != null) deckField.Click_Card.Object_name = "";
    }
    void OnMouseDown()
    {
        if(Goblin_cardUse && deckField.Click_Card.Object_name == "Goblin")
        {
            deckField.Click_Card.Card_MouseClick = false;
            Card_Damage = deckField.Click_Card.Card_status;
            deckField.Click_Card.CardDestroy();
            deckField.Click_Card = null;
            deck.CardCount = 0;
            deckField.cardHide = false;
            player.animation_Attack = true;
            player.targetPlayerCard = false;
            targetGoblinCard = false;
            HIT_Goblin = true;
        }
    }
    void Goblin_Attack()
    {
        if (animator.GetBool("GoblinIdle"))
        {
            animator.SetBool("GoblinIdle", false);
            animator.SetBool("GoblinMove", true);
        }
        if (animator.GetBool("GoblinMove"))
        {
            if (transform.position.x > -10)
            {
                transform.position = new Vector3(transform.position.x - 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("GoblinMove", false);
                animator.SetBool("GoblinAttack", true);
                player.EnemyAttack_Player = true; // !+ 플레이어 피격 애니메이션 활성화
            }
        }
        if (animator.GetBool("GoblinAttack"))
        {
            if (Attack_timer < 1.15f)
            {
                Attack_timer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("GoblinAttack", false);
                animator.SetBool("GoblinBackMove", true);
                player.EnemyAttack_Player = false; // !+ 플레이어 피격 애니메이션 비활성화
                player.PlayerDamage = true;        // 플레이어HP 줄이기
                Attack_timer = 0f;
            }
        }
        if (animator.GetBool("GoblinBackMove"))
        {
            if (transform.position.x < animation_position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("GoblinBackMove", false);
                animator.SetBool("GoblinIdle", true);
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = animation_position;
                animation_Attack = true;
            }
        }
    }
    void Hit_Goblin()
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        if (player.PlayerAttack_Enemy)
        {
            animator.SetBool("GoblinHit", true);
            if (player.PlayerAttack_timer > 1f) GoblinDamage = true;
        }
        else
        {
            animator.SetBool("GoblinHit", false);
        }

        if (GoblinDamage)
        {
            nowHp -= Card_Damage;
            Card_Damage = 0;
            GoblinDamage = false;
        }

        if (nowHp <= 0f)
        {
            animator.SetTrigger("GoblinDie");
            if (Dead_timer < 0.5f) Dead_timer += Time.deltaTime;
            else
            {
                Destroy(gameObject);
                Destroy(hpbar.gameObject);
            }
        }
    }
    void animationPosition()
    {
        if (ObjectSet.Enemy_Name[0] == "Goblin")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[0];
        }
        if (ObjectSet.Enemy_Name[1] == "Goblin")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[1];
        }
        if (ObjectSet.Enemy_Name[2] == "Goblin")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[2];
        }
        if (ObjectSet.Enemy_Name[3] == "Goblin")
        {
            Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
            hpbar.position = HpBarPos;
            animation_position = ObjectSet.Field_transform[3];
        }
    }
    void Attack_Order()
    {
        if (ObjectSet.Enemy_Name[0] == "Goblin" && attack_order.Order_1)
        {
            EnemyAttack = true;
            if(animation_Attack)
            {
                attack_order.Order_1 = false;
                attack_order.Order_2 = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
        if (ObjectSet.Enemy_Name[1] == "Goblin" && attack_order.Order_2)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_2 = false;
                attack_order.Order_3 = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
        if (ObjectSet.Enemy_Name[2] == "Goblin" && attack_order.Order_3)
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
        if (ObjectSet.Enemy_Name[3] == "Goblin" && attack_order.Order_4)
        {
            EnemyAttack = true;
            if (animation_Attack)
            {
                attack_order.Order_4 = false;
                ObjectSet.CardAdd = true;
                EnemyAttack = false;
                animation_Attack = false;
            }
        }
    }
}
