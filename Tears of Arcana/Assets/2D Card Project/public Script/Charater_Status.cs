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
                status = new Charater_Status(Charater_name, "플레이어", 10000, 100, 0, 0);
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
            case Charater_namedata.BlackSlime:
                status = new Charater_Status(Charater_name, "검은액체", 50, 0, 0, 10);
                break;
            case Charater_namedata.DeathBringer:
                status = new Charater_Status(Charater_name, "죽음을가져오는자", 120, 0, 0, 20);
                break;
            case Charater_namedata.FireWizard:
                status = new Charater_Status(Charater_name, "화염마법사", 70, 0, 0, 30);
                break;
            case Charater_namedata.Necromancer:
                status = new Charater_Status(Charater_name, "죽음술사", 100, 0, 0, 20);
                break;
            case Charater_namedata.FireWorm:
                status = new Charater_Status(Charater_name, "화염구렁이", 150, 0, 0, 20);
                break;
            case Charater_namedata.FlyDemon:
                status = new Charater_Status(Charater_name, "하급악마", 200, 0, 0, 20);
                break;
            case Charater_namedata.miniMushroom:
                status = new Charater_Status(Charater_name, "작은버섯괴물", 30, 0, 0, 10);
                break;
            case Charater_namedata.Bat:
                status = new Charater_Status(Charater_name, "동굴박쥐", 50, 0, 0, 10);
                break;
            case Charater_namedata.OrangeGolem:
                status = new Charater_Status(Charater_name, "토파즈골렘", 300, 0, 0, 10);
                break;
            case Charater_namedata.BlueGolem:
                status = new Charater_Status(Charater_name, "미스릴골렘", 300, 0, 0, 10);
                break;
            case Charater_namedata.Titan:
                status = new Charater_Status(Charater_name, "타이탄", 300, 0, 0, 10);
                break;
            case Charater_namedata.DemonSlime:
                status = new Charater_Status(Charater_name, "액체악마", 300, 0, 0, 10);
                break;
            case Charater_namedata.Argon:
                status = new Charater_Status(Charater_name, "아르곤", 300, 0, 0, 10);
                break;
            case Charater_namedata.AxeCastleGuardian:
                status = new Charater_Status(Charater_name, "성수호자01", 300, 0, 0, 10);
                break;
            case Charater_namedata.SpearCastleGuardian:
                status = new Charater_Status(Charater_name, "성수호자02", 300, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior01:
                status = new Charater_Status(Charater_name, "고스트워리어01", 300, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior02:
                status = new Charater_Status(Charater_name, "고스트워리어02", 300, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior03:
                status = new Charater_Status(Charater_name, "고스트워리어03", 300, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior04:
                status = new Charater_Status(Charater_name, "고스트워리어04", 300, 0, 0, 10);
                break;

            default:
                break;
        }
        return status;
    }
}
