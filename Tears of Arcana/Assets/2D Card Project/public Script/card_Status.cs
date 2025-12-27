using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card_Status
{
    public card_Name Card_name { get; }
    public string InputName { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    public card_Status()
    {

    }
    public card_Status(card_Name card_Name, string inputName, int damage, int health)
    {
        this.Card_name = card_Name;
        this.InputName = inputName;
        this.Damage = damage;
        this.Health = health;
    }
    public card_Status Card_inStatus(Card_namedata name)
    {
        card_Status status = null;

        switch(name)
        {
            case Card_namedata.HealthPotion_card:
                status = new card_Status(Card_name, "중급회복포션", 0, 5);
                break;
            case Card_namedata.HighHealthPotion_card:
                status = new card_Status(Card_name, "상급회복포션", 0, 15);
                break;
            case Card_namedata.Meditation:
                status = new card_Status(Card_name, "명상", 0, 30);
                break;
            case Card_namedata.Attack_card:
                status = new card_Status(Card_name, "일반물리공격", 15, 0);
                break;
            case Card_namedata.Magic_card:
                status = new card_Status(Card_name, "일반마법공격", 20, 0);
                break;
            case Card_namedata.Fire_card:
                status = new card_Status(Card_name, "불속성카드", 20, 0);
                break;
            case Card_namedata.Water_card:
                status = new card_Status(Card_name, "물속성카드", 20, 0);
                break;
            case Card_namedata.Wind_card:
                status = new card_Status(Card_name, "바람속성카드", 20, 0);
                break;
            case Card_namedata.Soil_card:
                status = new card_Status(Card_name, "흙속성카드", 20, 0);
                break;
            case Card_namedata.Devil_card:
                status = new card_Status(Card_name, "특수카드", 30, 0);
                break;
            default:
                break;
        }
        return status;
    }
}
