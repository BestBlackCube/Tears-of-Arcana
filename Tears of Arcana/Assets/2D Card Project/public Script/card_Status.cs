using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card_Status
{
    public card_Name Card_name { get; }
    public string InputName { get; set; }
    public int Single_Damage { get; set; }
    public int Multiple_Damage { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Count { get; set; }
    public card_Status()
    {

    }
    public card_Status(card_Name card_Name, string inputName, int single_damage, int multiple_damage,int health, int mana, int count)
    {
        this.Card_name = card_Name;
        this.InputName = inputName;
        this.Single_Damage = single_damage;
        this.Multiple_Damage = multiple_damage;
        this.Health = health;
        this.Mana = mana;
        this.Count = count;
    }
    public card_Status Card_inStatus(Card_namedata name)
    {
        card_Status status = null;

        switch(name)
        {
            case Card_namedata.Next:
                status = new card_Status(Card_name, "다음으로", 0, 0, 0, 0, 0);
                break;
            case Card_namedata.Return:
                status = new card_Status(Card_name, "이전으로", 0, 0, 0, 0, 0);
                break;
            case Card_namedata.HealthPotion:
                status = new card_Status(Card_name, "하급회복물약", 0, 0, 10, 0, 0);
                break;
            case Card_namedata.HighHealthPotion:
                status = new card_Status(Card_name, "상급회복물약", 0, 0, 30, 0, 0);
                break;
            case Card_namedata.Meditation:
                status = new card_Status(Card_name, "명상", 0, 0, 60, 10, 0);
                break;
            case Card_namedata.idleMagic:
                status = new card_Status(Card_name, "일반마법", 20, 0, 0, 5, 0);
                break;
            case Card_namedata.Fire:
                status = new card_Status(Card_name, "화염장판", 0, 20, 0, 10, 0);
                break;
            case Card_namedata.Water:
                status = new card_Status(Card_name, "얼음안개", 0, 20, 0, 10, 0);
                break;
            case Card_namedata.Wind:
                status = new card_Status(Card_name, "바람의창", 20, 0, 0, 5, 0);
                break;
            case Card_namedata.Soil:
                status = new card_Status(Card_name, "돌무더기", 10, 0, 0, 5, 0);
                break;
            case Card_namedata.FireOfvitality:
                status = new card_Status(Card_name, "생명의잔불", 0, 0, -20, 0, 2);
                break;
            case Card_namedata.Quietrest:
                status = new card_Status(Card_name, "고요한안식", 0, 0, 100, 0, 0);
                break;
            case Card_namedata.AbyssCrevice:
                status = new card_Status(Card_name, "절망의균열", 0, 0, 0, 20, 2);
                break;
            case Card_namedata.BrutalContract:
                status = new card_Status(Card_name, "잔혹한계약", 0, 0, 0, 30, 3);
                break;
            default:
                break;
        }
        return status;
    }
}
