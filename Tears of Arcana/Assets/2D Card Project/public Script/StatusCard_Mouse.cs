using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCard_Mouse : MonoBehaviour
{
    [SerializeField] ItemOption_Script itemOption;

    public bool OptionCard_Set = false;

    public int PlusHp = 0;
    public int PlusMp = 0;
    public int PlusDp = 0;
    public int PlusAp = 0;
    bool Card00 = false;
    bool Card01 = false;
    bool Card02 = false;
    private void OnMouseOver()
    {
        if (gameObject.name == "StatusCard00" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            Card00 = true;
        }
        if(gameObject.name == "StatusCard01" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            Card01 = true;
        }
        if(gameObject.name == "StatusCard02" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            Card02 = true;
        }
    }
    void OnMouseExit()
    {
        Card00 = false;
        Card01 = false;
        Card02 = false;
        if(OptionCard_Set) itemOption.ClickItem.itemBoxName = "";
    }
    private void OnMouseDown()
    {
        if (gameObject.name == "StatusCard00" && itemOption.ClickItem.itemname != null)
        {
            Item_inData(gameObject.name, itemOption.ClickItem.name);
            itemOption.ClickItem.itemBoxName = "";
            Card00 = false;
        }
        if (gameObject.name == "StatusCard01" && itemOption.ClickItem.itemname != null)
        {
            itemOption.ClickItem.itemBoxName = "";
            Card01 = false;
        }
        if (gameObject.name == "StatusCard02" && itemOption.ClickItem.itemname != null)
        {
            itemOption.ClickItem.itemBoxName = "";
            Card02 = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Item_inData(string BoxName, string itemName)
    {
        switch (BoxName)
        {
            case "StatusCard00":
                switch (itemName)
                {
                    case "아이템":
                        GameObject.Find(BoxName).GetComponent<StatusCard_Mouse>().PlusHp = itemOption.ClickItem.itemHp;
                        GameObject.Find(BoxName).GetComponent<StatusCard_Mouse>().PlusMp = itemOption.ClickItem.itemMp;
                        GameObject.Find(BoxName).GetComponent<StatusCard_Mouse>().PlusDp = itemOption.ClickItem.itemDp;
                        GameObject.Find(BoxName).GetComponent<StatusCard_Mouse>().PlusAp = itemOption.ClickItem.itemAp;
                        break;

                    default:
                        break;
                }
                break;
            case "StatusCard01":
                break;
            case "StatusCard02":
                break;

            default:
                break;
        }
    }
}
