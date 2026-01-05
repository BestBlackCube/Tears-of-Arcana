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
    [SerializeField] StatusCard_Mouse cardMouse;
    [SerializeField] Player_Script player;

    [SerializeField] bool Card_Use = false;
    [SerializeField] bool Battle_Use = false;
    [SerializeField] bool Card_Active = false;
    [SerializeField] bool Card_transformMouse = false;
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
        cardMouse = FindObjectOfType<StatusCard_Mouse>();
        player = FindObjectOfType<Player_Script>();

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
        if(Input.GetMouseButtonDown(1) && Card_Active)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            if (itemOption.ClickItem != null) itemOption.ClickItem = null;
            itemBoxName = "";
            Card_transformMouse = false;
            itemOption.item_inFieldCard = true;
            for (int i = 0; i < 3; i++)
            {
                itemOption.target_Field[i].SetActive(false);
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
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            Card_transformMouse = true;
        }
        if(Card_Use)
        {
            Card_Active = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            itemOption.BoxColliderActive(true);
            itemOption.item_inFieldCard = false;
            itemOption.ClickItem = this.gameObject.GetComponent<ItemCard_Script>();
            Card_transformMouse = true;

            for (int i = 0; i < 3; i++)
            {
                itemOption.target_Field[i].SetActive(true);
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = true;
            }
        }
    }
    void Card_Mouse()
    {
        if(Battle_Use)
        {
            switch (itemBoxName)
            {
                case "Player":
                    Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 10, 0);
                    transform.position = Vector3.Lerp(transform.position, playerPosition, 5 * Time.deltaTime);
                    break;
                default:
                    Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                    transform.position = Vector3.Lerp(transform.position, mousePosition, 10 * Time.deltaTime);
                    break;
            }
        }
        if(Card_Use)
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
                    Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8);
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                    transform.position = Vector3.Lerp(this.transform.position, mousePosition, 10 * Time.deltaTime);
                    break;
            }
        }
    }
}
