using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class StatusCard_Mouse : MonoBehaviour
{
    [SerializeField] ItemOption_Script itemOption;
    Player_Script player;
    public bool OptionCard_Set = false;

    public bool BoxInCard = false;

    public int PlusHp = 0;
    public int PlusMp = 0;
    public int PlusDp = 0;
    public int PlusAp = 0;
    private void OnMouseOver()
    {
        if (gameObject.name == "StatusCard00" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            itemOption.GetComponent<ItemOption_Script>().target_Field[0].SetActive(false);
        }
        if(gameObject.name == "StatusCard01" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            itemOption.GetComponent<ItemOption_Script>().target_Field[1].SetActive(false);
        }
        if(gameObject.name == "StatusCard02" && OptionCard_Set)
        {
            itemOption.ClickItem.itemBoxName = gameObject.name;
            itemOption.GetComponent<ItemOption_Script>().target_Field[2].SetActive(false);
        }
    }
    void OnMouseExit()
    {
        for(int i  = 0; i < 3; i++) if (!BoxInCard) itemOption.GetComponent<ItemOption_Script>().target_Field[i].SetActive(true);
        if (OptionCard_Set) itemOption.ClickItem.itemBoxName = "";
    }
    private void OnMouseDown()
    {
        if (gameObject.name == "StatusCard00" && itemOption.ClickItem.itemname != null)
        {
            itemOption.ClickItem.GetComponent<ItemCard_Script>().Status00 = true;
            for (int i = 0; i < itemOption.InField_nowitemCard.Length; i++) if (itemOption.InField_nowitemCard[i] != null)
                    itemOption.InField_nowitemCard[i].GetComponent<BoxCollider2D>().enabled = true;

            GameObject.Find("StatusCard00").GetComponent<StatusCard_Mouse>().PlusHp = itemOption.ClickItem.itemHp;
            GameObject.Find("StatusCard00").GetComponent<StatusCard_Mouse>().PlusMp = itemOption.ClickItem.itemMp;
            GameObject.Find("StatusCard00").GetComponent<StatusCard_Mouse>().PlusDp = itemOption.ClickItem.itemDp;
            GameObject.Find("StatusCard00").GetComponent<StatusCard_Mouse>().PlusAp = itemOption.ClickItem.itemAp;

            player.maxHp += PlusHp;
            player.maxMp += PlusMp;
            player.Defence_percent += PlusDp;
            player.Avoid_percent += PlusAp;

            itemOption.HpPlus_Persent += PlusHp;
            itemOption.MpPlus_Persent += PlusMp;
            itemOption.Defence_Persent += PlusDp;
            itemOption.Avoid_Persent += PlusAp;

            for (int i = 0; i < 3; i++)
            {
                itemOption.StatusCard_Object[i].GetComponent<BoxCollider2D>().enabled = false;
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = false;
                itemOption.target_Field[i].SetActive(false);
            }
            itemOption.ClickItem.itemBoxName = "";
            itemOption.ClickItem = null;
            this.gameObject.GetComponent<StatusCard_Mouse>().BoxInCard = true;
        }
        if (gameObject.name == "StatusCard01" && itemOption.ClickItem.itemname != null)
        {
            itemOption.ClickItem.GetComponent<ItemCard_Script>().Status01 = true;
            for (int i = 0; i < itemOption.InField_nowitemCard.Length; i++) if (itemOption.InField_nowitemCard[i] != null)
                    itemOption.InField_nowitemCard[i].GetComponent<BoxCollider2D>().enabled = true;

            GameObject.Find("StatusCard01").GetComponent<StatusCard_Mouse>().PlusHp = itemOption.ClickItem.itemHp;
            GameObject.Find("StatusCard01").GetComponent<StatusCard_Mouse>().PlusMp = itemOption.ClickItem.itemMp;
            GameObject.Find("StatusCard01").GetComponent<StatusCard_Mouse>().PlusDp = itemOption.ClickItem.itemDp;
            GameObject.Find("StatusCard01").GetComponent<StatusCard_Mouse>().PlusAp = itemOption.ClickItem.itemAp;

            player.maxHp += PlusHp;
            player.maxMp += PlusMp;
            player.Defence_percent += PlusDp;
            player.Avoid_percent += PlusAp;

            itemOption.HpPlus_Persent += PlusHp;
            itemOption.MpPlus_Persent += PlusMp;
            itemOption.Defence_Persent += PlusDp;
            itemOption.Avoid_Persent += PlusAp;

            for (int i = 0; i < 3; i++)
            {
                itemOption.StatusCard_Object[i].GetComponent<BoxCollider2D>().enabled = false;
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = false;
                itemOption.target_Field[i].SetActive(false);
            }
            this.gameObject.GetComponent<StatusCard_Mouse>().BoxInCard = true; 
            itemOption.ClickItem.itemBoxName = "";
            itemOption.ClickItem = null;
        }
        if (gameObject.name == "StatusCard02" && itemOption.ClickItem.itemname != null)
        {
            itemOption.ClickItem.GetComponent<ItemCard_Script>().Status02 = true;
            for (int i = 0; i < itemOption.InField_nowitemCard.Length; i++) if (itemOption.InField_nowitemCard[i] != null) 
                itemOption.InField_nowitemCard[i].GetComponent<BoxCollider2D>().enabled = true;

            GameObject.Find("StatusCard02").GetComponent<StatusCard_Mouse>().PlusHp = itemOption.ClickItem.itemHp;
            GameObject.Find("StatusCard02").GetComponent<StatusCard_Mouse>().PlusMp = itemOption.ClickItem.itemMp;
            GameObject.Find("StatusCard02").GetComponent<StatusCard_Mouse>().PlusDp = itemOption.ClickItem.itemDp;
            GameObject.Find("StatusCard02").GetComponent<StatusCard_Mouse>().PlusAp = itemOption.ClickItem.itemAp;

            player.maxHp += PlusHp;
            player.maxMp += PlusMp;
            player.Defence_percent += PlusDp;
            player.Avoid_percent += PlusAp;

            itemOption.HpPlus_Persent += PlusHp;
            itemOption.MpPlus_Persent += PlusMp;
            itemOption.Defence_Persent += PlusDp;
            itemOption.Avoid_Persent += PlusAp;

            for (int i = 0; i < 3; i++)
            {
                itemOption.StatusCard_Object[i].GetComponent<BoxCollider2D>().enabled = false;
                itemOption.StatusCard_Object[i].GetComponent<StatusCard_Mouse>().OptionCard_Set = false;
                itemOption.target_Field[i].SetActive(false);
            }
            this.gameObject.GetComponent<StatusCard_Mouse>().BoxInCard = true; 
            itemOption.ClickItem.itemBoxName = "";
            itemOption.ClickItem = null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if(BoxInCard)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            for(int j = 0; j < 3; j++)
            {
                if (itemOption.StatusCard_Object[j].GetComponent<StatusCard_Mouse>().BoxInCard)
                    itemOption.target_Field[j].SetActive(false);
            }
            for(int i = 0; i < itemOption.InField_nowitemCard.Length; i++)
            {
                if (itemOption.InField_nowitemCard[i] != null)
                {
                    if (itemOption.InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status00 ||
                    itemOption.InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status01 ||
                    itemOption.InField_nowitemCard[i].GetComponent<ItemCard_Script>().Status02)
                    {
                        itemOption.InField_nowitemCard[i].GetComponent<ItemCard_Script>().Card_Active = false;
                        itemOption.InField_nowitemCard[i].GetComponent<ItemCard_Script>().Card_transformMouse = false;
                    }
                }
            }
            
        }

    }
}
