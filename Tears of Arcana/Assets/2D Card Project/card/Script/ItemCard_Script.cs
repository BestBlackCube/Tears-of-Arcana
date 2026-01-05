using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemCard_Script : MonoBehaviour
{
    [SerializeField] Item_Carddata itemName;
    [SerializeField] card_Status item;
    [SerializeField] ItemOption_Script itemOption;
    [SerializeField] Player_Script player;

    [SerializeField] bool Card_Use = false;
    [SerializeField] bool Battle_Use = false;

    public bool Status00 = false;
    public bool Status01 = false;
    public bool Status02 = false;
    public bool Card_Active = false;
    public bool Battle_Active = false;
    public bool Card_transformMouse = false;
    public string itemBoxName;

    public string itemname;
    public int itemHp;
    public int itemMp;
    public int itemDp;
    public int itemAp;
    // Start is called before the first frame update
    void Start()
    {
        itemOption = FindObjectOfType<ItemOption_Script>();
        player = GameObject.Find("Player").GetComponent<Player_Script>();

        item = new card_Status();
        item = item.item_Card(itemName);
        itemname = item.InputName;
        itemHp = item.Health;
        itemMp = item.Mana;
        itemDp = item.Single_Damage;
        itemAp = item.Multiple_Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Battle_Active)
        {
            for (int j = 0; j < itemOption.InField_nowitemCard.Length; j++) if (itemOption.InField_nowitemCard[j] != null)
                    itemOption.InField_nowitemCard[j].GetComponent<BoxCollider2D>().enabled = true;
            itemBoxName = "";
            Card_transformMouse = false;
            Player_target(false);
            Battle_Active = false;
        }
        if(Input.GetMouseButtonDown(1) && Card_Active)
        {
            for (int j = 0; j < itemOption.InField_nowitemCard.Length; j++) if (itemOption.InField_nowitemCard[j] != null)
                    itemOption.InField_nowitemCard[j].GetComponent<BoxCollider2D>().enabled = true;
            if (itemOption.ClickItem != null) itemOption.ClickItem = null;
            itemBoxName = "";
            Card_transformMouse = false;
            for (int i = 0; i < 3; i++)
            {
                itemOption.target_Field[i].SetActive(false);
                itemOption.StatusCard_Object[i].GetComponent<BoxCollider2D>().enabled = false;
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = false;
            }
            Card_Active = false;
        }
        if (Card_transformMouse) Card_Mouse();
    }
    private void OnMouseOver()
    {
        if (this.itemname == "전장으로") Battle_Use = true;
        else Card_Use = true;
    }
    private void OnMouseExit()
    {
        if (this.itemname == "전장으로") Battle_Use = false;
        else Card_Use = false;
    }
    private void OnMouseDown()
    {
        if(Battle_Use)
        {
            Battle_Active = true;
            for (int j = 0; j < itemOption.InField_nowitemCard.Length; j++) if (itemOption.InField_nowitemCard[j] != null)
                    itemOption.InField_nowitemCard[j].GetComponent<BoxCollider2D>().enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            Player_target(true);
            Card_transformMouse = true;
        }
        if(Card_Use)
        {
            Card_Active = true;
            for (int j = 0; j < itemOption.InField_nowitemCard.Length; j++) if (itemOption.InField_nowitemCard[j] != null) 
                    itemOption.InField_nowitemCard[j].GetComponent<BoxCollider2D>().enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            itemOption.BoxColliderActive(true);
            itemOption.ClickItem = this.gameObject.GetComponent<ItemCard_Script>();
            Card_transformMouse = true;

            for (int i = 0; i < 3; i++)
            {
                if (!itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().BoxInCard) itemOption.target_Field[i].SetActive(true);
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = true;
            }
            StatusDelete();
        }
    }
    void Card_Mouse()
    {
        if(Battle_Active)
        {
            switch (itemBoxName)
            {
                case "Player":
                    Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 6, 0);
                    this.transform.position = Vector3.Lerp(transform.position, playerPosition, 5 * Time.deltaTime);
                    break;
                default:
                    Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6);
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                    this.transform.position = Vector3.Lerp(transform.position, mousePosition, 10 * Time.deltaTime);
                    break;
            }
        }
        if(Card_Active)
        {
            switch (itemBoxName)
            {
                case "StatusCard00":
                    Vector3 Statusbox00 = new Vector3(GameObject.Find(itemBoxName).transform.position.x,
                        GameObject.Find(itemBoxName).transform.position.y, -5);
                    itemOption.ClickItem.transform.position = Vector3.Lerp(itemOption.ClickItem.transform.position, Statusbox00, 10 * Time.deltaTime);
                    break;
                case "StatusCard01":
                    Vector3 Statusbox01 = new Vector3(GameObject.Find(itemBoxName).transform.position.x,
                        GameObject.Find(itemBoxName).transform.position.y, -5);
                    itemOption.ClickItem.transform.position = Vector3.Lerp(itemOption.ClickItem.transform.position, Statusbox01, 10 * Time.deltaTime);
                    break;
                case "StatusCard02":
                    Vector3 Statusbox02 = new Vector3(GameObject.Find(itemBoxName).transform.position.x,
                        GameObject.Find(itemBoxName).transform.position.y, -5);
                    itemOption.ClickItem.transform.position = Vector3.Lerp(itemOption.ClickItem.transform.position, Statusbox02, 10 * Time.deltaTime);
                    break;

                default:
                    Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7);
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                    itemOption.ClickItem.transform.position = Vector3.Lerp(this.transform.position, mousePosition, 10 * Time.deltaTime);
                    break;
            }
        }
    }
    void StatusDelete()
    {
        if(Status00)
        {
            player.maxHp -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusHp;
            player.maxMp -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusMp;
            player.Defence_percent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusDp;
            player.Avoid_percent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.HpPlus_Persent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusHp;
            itemOption.MpPlus_Persent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusMp;
            itemOption.Defence_Persent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusDp;
            itemOption.Avoid_Persent -= itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusHp = 0;
            itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusMp = 0;
            itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusDp = 0;
            itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().PlusAp = 0;

            itemOption.StatusCard_Object[0].GetComponent<StatusCard_Mouse>().BoxInCard = false;
            Status00 = false;
        }
        if(Status01)
        {
            player.maxHp -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusHp;
            player.maxMp -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusMp;
            player.Defence_percent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusDp;
            player.Avoid_percent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.HpPlus_Persent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusHp;
            itemOption.MpPlus_Persent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusMp;
            itemOption.Defence_Persent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusDp;
            itemOption.Avoid_Persent -= itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusHp = 0;
            itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusMp = 0;
            itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusDp = 0;
            itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().PlusAp = 0;

            itemOption.StatusCard_Object[1].GetComponent<StatusCard_Mouse>().BoxInCard = false;
            Status01 = false;
        }
        if(Status02)
        {
            player.maxHp -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusHp;
            player.maxMp -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusMp;
            player.Defence_percent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusDp;
            player.Avoid_percent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.HpPlus_Persent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusHp;
            itemOption.MpPlus_Persent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusMp;
            itemOption.Defence_Persent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusDp;
            itemOption.Avoid_Persent -= itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusAp;

            itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusHp = 0;
            itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusMp = 0;
            itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusDp = 0;
            itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().PlusAp = 0;

            itemOption.StatusCard_Object[2].GetComponent<StatusCard_Mouse>().BoxInCard = false;
            Status02 = false;
        }

    }
    public void Player_target(bool Click)
    {
        if (Click)
        {
            player.Arrow = true;
            player.BattleCard = this.gameObject;
        }
        else
        {
            player.Arrow = false;
            player.BattleCard = null;
        }
    }
}
