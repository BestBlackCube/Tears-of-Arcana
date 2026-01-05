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
                status = new Charater_Status(Charater_name, "눈괴물", 30, 0, 0, 5);
                break;
            case Charater_namedata.Goblin:
                status = new Charater_Status(Charater_name, "고블린", 50, 0, 0, 5);
                break;
            case Charater_namedata.Mushroom:
                status = new Charater_Status(Charater_name, "버섯괴물", 100, 0, 0, 10);
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
                status = new Charater_Status(Charater_name, "죽음술사", 60, 0, 0, 20);
                break;
            case Charater_namedata.FireWorm:
                status = new Charater_Status(Charater_name, "화염구렁이", 70, 0, 0, 10);
                break;
            case Charater_namedata.FlyDemon:
                status = new Charater_Status(Charater_name, "하급악마", 200, 0, 0, 20);
                break;
            case Charater_namedata.miniMushroom:
                status = new Charater_Status(Charater_name, "작은버섯괴물", 30, 0, 0, 5);
                break;
            case Charater_namedata.Bat:
                status = new Charater_Status(Charater_name, "동굴박쥐", 40, 0, 0, 10);
                break;
            case Charater_namedata.OrangeGolem:
                status = new Charater_Status(Charater_name, "토파즈골렘", 300, 0, 0, 10);
                break;
            case Charater_namedata.BlueGolem:
                status = new Charater_Status(Charater_name, "미스릴골렘", 300, 0, 0, 10);
                break;
            case Charater_namedata.Titan:
                status = new Charater_Status(Charater_name, "타이탄", 500, 0, 0, 40);
                break;
            case Charater_namedata.DemonSlime:
                status = new Charater_Status(Charater_name, "액체악마", 700, 0, 0, 50);
                break;
            case Charater_namedata.Argon:
                status = new Charater_Status(Charater_name, "아르곤", 350, 0, 0, 15);
                break;
            case Charater_namedata.AxeCastleGuardian:
                status = new Charater_Status(Charater_name, "성수호자01", 700, 0, 0, 10);
                break;
            case Charater_namedata.SpearCastleGuardian:
                status = new Charater_Status(Charater_name, "성수호자02", 700, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior01:
                status = new Charater_Status(Charater_name, "고스트워리어01", 120, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior02:
                status = new Charater_Status(Charater_name, "고스트워리어02", 130, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior03:
                status = new Charater_Status(Charater_name, "고스트워리어03", 100, 0, 0, 10);
                break;
            case Charater_namedata.GhostWarrior04:
                status = new Charater_Status(Charater_name, "고스트워리어04", 120, 0, 0, 10);
                break;
            case Charater_namedata.Goblin_Nomal:
                status = new Charater_Status(Charater_name, "일반고블린", 50, 0, 0, 5);
                break;
            case Charater_namedata.Goblin_Assassin:
                status = new Charater_Status(Charater_name, "암살자고블린", 40, 0, 0, 15);
                break;
            case Charater_namedata.Goblin_Mage:
                status = new Charater_Status(Charater_name, "마법사고블린", 50, 0, 0, 20);
                break;
            case Charater_namedata.Goblin_Spear:
                status = new Charater_Status(Charater_name, "창고블린", 30, 0, 0, 15);
                break;
            case Charater_namedata.Goblin_Soldier:
                status = new Charater_Status(Charater_name, "홉고블린", 60, 0, 0, 15);
                break;
            case Charater_namedata.CastleKnight00:
                status = new Charater_Status(Charater_name, "성기사00", 150, 0, 0, 25);
                break;
            case Charater_namedata.CastleKnight01:
                status = new Charater_Status(Charater_name, "성시가01", 150, 0, 0, 25);
                break;
            case Charater_namedata.BlueSlime:
                status = new Charater_Status(Charater_name, "파란액체", 20, 0, 0, 5);
                break;
            case Charater_namedata.GreenSlime:
                status = new Charater_Status(Charater_name, "초록액체", 20, 0, 0, 5);
                break;
            case Charater_namedata.RedSlime:
                status = new Charater_Status(Charater_name, "붉은액체", 20, 0, 0, 5);
                break;
            case Charater_namedata.ArgonDommy:
                status = new Charater_Status(Charater_name, "아르곤분신", 70, 0, 0, 5);
                break;

            default:
                break;
        }
        return status;
    }
}
