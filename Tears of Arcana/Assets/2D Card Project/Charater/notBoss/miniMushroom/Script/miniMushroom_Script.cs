using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniMushroom_Script : MonoBehaviour
{
    public bool Guide = false;
    public bool Arrow = false;

    public GameObject HpBar_prefab;
    public GameObject canvas;

    public Charater_namedata unitname;
    public Charater_Status status;

    RectTransform hpbar;
    Animator animator;

    public int Card_Damage;
    public int nowHp;
    public int maxHp;
    public int Dmg;

    public int stun_count;
    public bool stun_countDown = false;

    public float Dead_timer = 0f;
    public float Attack_timer = 0f;

    EnemyObjectSet_Script ObjectSet;
    ObjectSet_Script attack_order;
    Player_Script player;
    CardDeckField_Script deckField;
    CardDeck_Script deck;
    //public Card_Script card;
    Image nowHpbar;

    public bool targetCard = false;
    public bool EnemyDamage = false;
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
        Dmg = status.Damage;
        canvas = GameObject.Find("HPCanvas");

        ObjectSet = FindObjectOfType<EnemyObjectSet_Script>();
        attack_order = FindObjectOfType<ObjectSet_Script>();

        animationPosition(1);

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
        if (EnemyAttack) Attack();  // 공격
        if (HIT_Enemy) Hit_Enemy();    // 맞기
        if (targetCard && !Arrow || !targetCard && !Arrow) targetArrow_inField(2);
        if (targetCard && Arrow) targetArrow_inField(1);
    }
    void OnMouseOver()
    {
        cardUse = true;
        if (targetCard && deckField.Click_Card != null)
        {
            deckField.Click_Card.Object_name = "miniMushroom";
            Arrow = false;
        }
        if (targetCard && deckField.Click_Card != null)
        {
            animationPosition(2);
            Guide = false;
        }
        if (!targetCard && !Guide) animationPosition(3);
    }
    void OnMouseExit()
    {
        cardUse = false;
        if (deckField.Click_Card != null && !Arrow) Arrow = true;
        if (targetCard && deckField.Click_Card != null) deckField.Click_Card.Object_name = "";
        if (!Guide) animationPosition(3);
    }
    void OnMouseDown()
    {
        if (deckField.Click_Card != null)
            if (cardUse && deckField.Click_Card.Object_name == "miniMushroom")
            {
                deckField.Click_Card.Card_MouseClick = false;
                deckField.Click_Card.Target_Card(false);
                CardData_inEnemy(deckField.Click_Card.Card_name);
                deckField.Click_Card.CardDestroy();
                deckField.Click_Card = null;
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
            if (Attack_timer < 1f)
            {
                Attack_timer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("Attack", false);
                animator.SetBool("BackMove", true);
                player.EnemyAttack_Player = false; // !+ 플레이어 피격 애니메이션 비활성화
                player.HitDamage = Dmg;
                player.PlayerDamage = true;        // 플레이어HP 줄이기
                Attack_timer = 0f;
            }
        }
        if (animator.GetBool("BackMove"))
        {
            if (transform.position.x < animation_position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = new Vector3(transform.position.x + 15f * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                animator.SetBool("BackMove", false);
                animator.SetBool("Idle", true);
                transform.localScale = new Vector3(1, 1, 1);
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
            deckField.cardHide = false;
            nowHp -= Card_Damage;
            Card_Damage = 0;
            EnemyDamage = false;
        }

        if (nowHp <= 0f)
        {
            animator.SetTrigger("Die");
            if (Dead_timer < 0.4f) Dead_timer += Time.deltaTime;
            else
            {
                ObjectSet.MonsterDeadCount++;
                Enemy_NameLess();
                Destroy(gameObject);
                Destroy(hpbar.gameObject);
            }
        }
    }
    void animationPosition(int Range)
    {
        if (Range == 1)
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.Enemy_Name[0] == "miniMushroom")
            {
                if (hpbar == null)
                {
                    hpbar = Instantiate(HpBar_prefab, canvas.transform).GetComponent<RectTransform>();
                    Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
                    hpbar.position = HpBarPos;
                    animation_position = ObjectSet.Field_transform[0];
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.Enemy_Name[1] == "miniMushroom")
            {
                if (hpbar == null)
                {
                    hpbar = Instantiate(HpBar_prefab, canvas.transform).GetComponent<RectTransform>();
                    Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
                    hpbar.position = HpBarPos;
                    animation_position = ObjectSet.Field_transform[1];
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.Enemy_Name[2] == "miniMushroom")
            {
                if (hpbar == null)
                {
                    hpbar = Instantiate(HpBar_prefab, canvas.transform).GetComponent<RectTransform>();
                    Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 3.5f, 0);
                    hpbar.position = HpBarPos;
                    animation_position = ObjectSet.Field_transform[2];
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                if (hpbar == null)
                {
                    hpbar = Instantiate(HpBar_prefab, canvas.transform).GetComponent<RectTransform>();
                    Vector3 HpBarPos = new Vector3(transform.position.x, transform.position.y - 5f, 0);
                    hpbar.position = HpBarPos;
                    animation_position = ObjectSet.Field_transform[3];
                }
            }
        }
        if (Range == 2)
        {
            Vector3 guide_offset = new Vector3(0, 0, 0);
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.TargetGuide[0] == null && ObjectSet.Enemy_Name[0] == "miniMushroom")
            {
                ObjectSet.TargetGuide[0] = Instantiate(ObjectSet.TargetGuide_prefab, guide_offset, Quaternion.identity);
                EnemyTargetBar_Script guide = ObjectSet.TargetGuide[0].GetComponent<EnemyTargetBar_Script>();
                guide.target = ObjectSet.Field_inMonster[0].transform;
                deckField.Click_Card.Card_transform = ObjectSet.Field_inMonster[0].transform.position;
                deckField.Click_Card.Card_upNumber = 0;
                switch (deckField.Click_Card.Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                    case "불화살":
                    case "전격":
                    case "고드름":
                        guide.offset[0] = new Vector3(-2f, 2f, 0);
                        guide.offset[1] = new Vector3(2f, 2f, 0);
                        guide.offset[2] = new Vector3(-2f, -2f, 0);
                        guide.offset[3] = new Vector3(2f, -2f, 0);
                        break;

                    case "화염장판":
                    case "얼음안개":
                        guide.offset[0] = new Vector3(-1.5f, 3f, 0);
                        guide.offset[1] = new Vector3(23f, 3f, 0);
                        guide.offset[2] = new Vector3(-1.5f, -3f, 0);
                        guide.offset[3] = new Vector3(23f, -3f, 0);
                        break;

                    default:
                        break;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.TargetGuide[1] == null && ObjectSet.Enemy_Name[1] == "miniMushroom")
            {
                ObjectSet.TargetGuide[1] = Instantiate(ObjectSet.TargetGuide_prefab, guide_offset, Quaternion.identity);
                EnemyTargetBar_Script guide = ObjectSet.TargetGuide[1].GetComponent<EnemyTargetBar_Script>();
                guide.target = ObjectSet.Field_inMonster[1].transform;
                deckField.Click_Card.Card_transform = ObjectSet.Field_inMonster[1].transform.position;
                deckField.Click_Card.Card_upNumber = 1;
                switch (deckField.Click_Card.Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                    case "불화살":
                    case "전격":
                    case "고드름":
                        guide.offset[0] = new Vector3(-2f, 2f, 0);
                        guide.offset[1] = new Vector3(2f, 2f, 0);
                        guide.offset[2] = new Vector3(-2f, -2f, 0);
                        guide.offset[3] = new Vector3(2f, -2f, 0);
                        break;

                    case "화염장판":
                    case "얼음안개":
                        guide.offset[0] = new Vector3(-8.5f, 3f, 0);
                        guide.offset[1] = new Vector3(16f, 3f, 0);
                        guide.offset[2] = new Vector3(-8.5f, -3f, 0);
                        guide.offset[3] = new Vector3(16f, -3f, 0);
                        break;

                    default:
                        break;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.TargetGuide[2] == null && ObjectSet.Enemy_Name[2] == "miniMushroom")
            {
                ObjectSet.TargetGuide[2] = Instantiate(ObjectSet.TargetGuide_prefab, guide_offset, Quaternion.identity);
                EnemyTargetBar_Script guide = ObjectSet.TargetGuide[2].GetComponent<EnemyTargetBar_Script>();
                guide.target = ObjectSet.Field_inMonster[2].transform;
                deckField.Click_Card.Card_transform = ObjectSet.Field_inMonster[2].transform.position;
                deckField.Click_Card.Card_upNumber = 2;
                switch (deckField.Click_Card.Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                    case "불화살":
                    case "전격":
                    case "고드름":
                        guide.offset[0] = new Vector3(-2f, 2f, 0);
                        guide.offset[1] = new Vector3(2f, 2f, 0);
                        guide.offset[2] = new Vector3(-2f, -2f, 0);
                        guide.offset[3] = new Vector3(2f, -2f, 0);
                        break;

                    case "화염장판":
                    case "얼음안개":
                        guide.offset[0] = new Vector3(-15.5f, 3f, 0);
                        guide.offset[1] = new Vector3(9f, 3f, 0);
                        guide.offset[2] = new Vector3(-15.5f, -3f, 0);
                        guide.offset[3] = new Vector3(9f, -3f, 0);
                        break;

                    default:
                        break;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.TargetGuide[3] == null && ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                ObjectSet.TargetGuide[3] = Instantiate(ObjectSet.TargetGuide_prefab, guide_offset, Quaternion.identity);
                EnemyTargetBar_Script guide = ObjectSet.TargetGuide[3].GetComponent<EnemyTargetBar_Script>();
                guide.target = ObjectSet.Field_inMonster[3].transform;
                deckField.Click_Card.Card_transform = ObjectSet.Field_inMonster[3].transform.position;
                deckField.Click_Card.Card_upNumber = 3;
                switch (deckField.Click_Card.Card_name)
                {
                    case "일반마법":
                    case "바람의창":
                    case "돌무더기":
                    case "절망의균열":
                    case "불화살":
                    case "전격":
                    case "고드름":
                        guide.offset[0] = new Vector3(-2f, 2f, 0);
                        guide.offset[1] = new Vector3(2f, 2f, 0);
                        guide.offset[2] = new Vector3(-2f, -2f, 0);
                        guide.offset[3] = new Vector3(2f, -2f, 0);
                        break;

                    case "화염장판":
                    case "얼음안개":
                        guide.offset[0] = new Vector3(-22.5f, 3f, 0);
                        guide.offset[1] = new Vector3(2f, 3f, 0);
                        guide.offset[2] = new Vector3(-22.5f, -3f, 0);
                        guide.offset[3] = new Vector3(2f, -3f, 0);
                        break;

                    default:
                        break;
                }
            }
        }
        if (Range == 3)
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.TargetGuide[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetGuide[0]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.TargetGuide[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetGuide[1]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.TargetGuide[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetGuide[2]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.TargetGuide[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetGuide[3]);
            }

        }
    }
    void targetArrow_inField(int Range)
    {
        if (Range == 1)
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.TargetArrow[0] == null && ObjectSet.Enemy_Name[0] == "miniMushroom")
            {
                Vector3 player_offset = new Vector3(animation_position.x, 6, 0);
                ObjectSet.TargetArrow[0] = Instantiate(ObjectSet.TargetArrow_prefab, player_offset, Quaternion.identity);
                TargetArrow_Script arrow = ObjectSet.TargetArrow[0].GetComponent<TargetArrow_Script>();
                arrow.offset = player_offset;
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.TargetArrow[1] == null && ObjectSet.Enemy_Name[1] == "miniMushroom")
            {
                Vector3 player_offset = new Vector3(animation_position.x, 6, 0);
                ObjectSet.TargetArrow[1] = Instantiate(ObjectSet.TargetArrow_prefab, player_offset, Quaternion.identity);
                TargetArrow_Script arrow = ObjectSet.TargetArrow[1].GetComponent<TargetArrow_Script>();
                arrow.offset = player_offset;
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.TargetArrow[2] == null && ObjectSet.Enemy_Name[2] == "miniMushroom")
            {
                Vector3 player_offset = new Vector3(animation_position.x, 6, 0);
                ObjectSet.TargetArrow[2] = Instantiate(ObjectSet.TargetArrow_prefab, player_offset, Quaternion.identity);
                TargetArrow_Script arrow = ObjectSet.TargetArrow[2].GetComponent<TargetArrow_Script>();
                arrow.offset = player_offset;
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.TargetArrow[3] == null && ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                Vector3 player_offset = new Vector3(animation_position.x, 6, 0);
                ObjectSet.TargetArrow[3] = Instantiate(ObjectSet.TargetArrow_prefab, player_offset, Quaternion.identity);
                TargetArrow_Script arrow = ObjectSet.TargetArrow[3].GetComponent<TargetArrow_Script>();
                arrow.offset = player_offset;
            }
        }
        if (Range == 2)
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.TargetArrow[0] != null && ObjectSet.Enemy_Name[0] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetArrow[0]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.TargetArrow[1] != null && ObjectSet.Enemy_Name[1] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetArrow[1]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.TargetArrow[2] != null && ObjectSet.Enemy_Name[2] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetArrow[2]);
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.TargetArrow[3] != null && ObjectSet.Enemy_Name[3] == "miniMushroom")
            {
                Destroy(ObjectSet.TargetArrow[3]);
            }

        }
    }
    void Attack_Order()
    {
        if (stun_count != 0)
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.Enemy_Name[0] == "miniMushroom" && attack_order.Order_1)
            {
                stun_countDown = true;
                if (stun_countDown)
                {
                    stun_count--;
                    attack_order.Order_1 = false;
                    if (ObjectSet.Field_inMonster[1] != null) { attack_order.Order_2 = true; stun_countDown = false; }
                    else if (ObjectSet.Field_inMonster[2] != null) { attack_order.Order_3 = true; stun_countDown = false; }
                    else if (ObjectSet.Field_inMonster[3] != null) { attack_order.Order_4 = true; stun_countDown = false; }
                    else { attack_order.CardAdd = true; stun_countDown = false; }
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.Enemy_Name[1] == "miniMushroom" && attack_order.Order_2)
            {
                stun_countDown = true;
                if (stun_countDown)
                {
                    stun_count--;
                    attack_order.Order_2 = false;
                    if (ObjectSet.Field_inMonster[2] != null) { attack_order.Order_3 = true; stun_countDown = false; }
                    else if (ObjectSet.Field_inMonster[3] != null) { attack_order.Order_4 = true; stun_countDown = false; }
                    else { attack_order.CardAdd = true; stun_countDown = false; }
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.Enemy_Name[2] == "miniMushroom" && attack_order.Order_3)
            {
                stun_countDown = true;
                if (stun_countDown)
                {
                    stun_count--;
                    attack_order.Order_3 = false;
                    if (ObjectSet.Field_inMonster[3] != null) { attack_order.Order_4 = true; stun_countDown = false; }
                    else { attack_order.CardAdd = true; stun_countDown = false; }
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.Enemy_Name[3] == "miniMushroom" && attack_order.Order_4)
            {
                stun_countDown = true;
                if (stun_countDown)
                {
                    stun_count--;
                    attack_order.Order_4 = false;
                    attack_order.CardAdd = true;
                    stun_countDown = false;
                }
            }
        }
        else
        {
            if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.Enemy_Name[0] == "miniMushroom" && attack_order.Order_1)
            {
                EnemyAttack = true;
                if (animation_Attack)
                {
                    attack_order.Order_1 = false;
                    if (ObjectSet.Field_inMonster[1] != null) attack_order.Order_2 = true;
                    else if (ObjectSet.Field_inMonster[2] != null) attack_order.Order_3 = true;
                    else if (ObjectSet.Field_inMonster[3] != null) attack_order.Order_4 = true;
                    else attack_order.CardAdd = true;
                    EnemyAttack = false;
                    animation_Attack = false;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.Enemy_Name[1] == "miniMushroom" && attack_order.Order_2)
            {
                EnemyAttack = true;
                if (animation_Attack)
                {
                    attack_order.Order_2 = false;
                    if (ObjectSet.Field_inMonster[2] != null) attack_order.Order_3 = true;
                    else if (ObjectSet.Field_inMonster[3] != null) attack_order.Order_4 = true;
                    else attack_order.CardAdd = true;
                    EnemyAttack = false;
                    animation_Attack = false;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.Enemy_Name[2] == "miniMushroom" && attack_order.Order_3)
            {
                EnemyAttack = true;
                if (animation_Attack)
                {
                    attack_order.Order_3 = false;
                    if (ObjectSet.Field_inMonster[3] != null) attack_order.Order_4 = true;
                    else attack_order.CardAdd = true;
                    EnemyAttack = false;
                    animation_Attack = false;
                }
            }
            if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.Enemy_Name[3] == "miniMushroom" && attack_order.Order_4)
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
    }
    void Enemy_NameLess()
    {
        if (this.gameObject == ObjectSet.Field_inMonster[0] && ObjectSet.Enemy_Name[0] == "miniMushroom") ObjectSet.Enemy_Name[0] = null;
        if (this.gameObject == ObjectSet.Field_inMonster[1] && ObjectSet.Enemy_Name[1] == "miniMushroom") ObjectSet.Enemy_Name[1] = null;
        if (this.gameObject == ObjectSet.Field_inMonster[2] && ObjectSet.Enemy_Name[2] == "miniMushroom") ObjectSet.Enemy_Name[2] = null;
        if (this.gameObject == ObjectSet.Field_inMonster[3] && ObjectSet.Enemy_Name[3] == "miniMushroom") ObjectSet.Enemy_Name[3] = null;
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
                stun_count += deckField.Click_Card.count;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "불화살":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "전격":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;
            case "고드름":
                Card_Damage = deckField.Click_Card.single_damage;
                player.nowMp += deckField.Click_Card.mana;
                break;

            default:
                break;
        }
    }
}
