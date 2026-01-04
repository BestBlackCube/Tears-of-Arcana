using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charater_Status
{
    public charater_Name Charater_name { get; }
    public string InputName { get; set; }
    public int MaxHp { get; set; }
    public int NowHp;
    public int MaxMp { get; set; }
    public int NowMp;

    public int Damage { get; set; }
    public Charater_Status()
    {

    }
    public Charater_Status(charater_Name Charater_name, string inputName, int maxHp, int maxMp, int nowMp, int damage)
    {
        this.Charater_name = Charater_name;
        this.InputName = inputName;
        this.MaxHp = maxHp;
        this.NowHp = maxHp;

        this.MaxMp = maxMp;
        this.NowMp = nowMp;
        this.Damage = damage;
    }
    public Charater_Status Char_inStatus(Charater_namedata name)
    {
        Charater_Status status = null;
        
        switch(name)
        {
            case Charater_namedata.Player:
                status = new Charater_Status(Charater_name, "플레이어", 100, 100, 0, 0);
                break;
            case Charater_namedata.Skeleton:
                status = new Charater_Status(Charater_name, "스켈레톤", 70, 0, 0, 10);
                break;
            case Charater_namedata.Eye:
                status = new Charater_Status(Charater_name, "눈괴물", 30, 0, 0, 10);
                break;
            case Charater_namedata.Goblin:
                status = new Charater_Status(Charater_name, "고블린", 50, 0, 0, 10);
                break;
            case Charater_namedata.Mushroom:
                status = new Charater_Status(Charater_name, "버섯괴물", 120, 0, 0, 10);
                break;

            default:
                break;
        }
        return status;
    }
}
