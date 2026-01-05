using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMotion_Script : MonoBehaviour
{
    public GameObject[] AttackMotion_Object;
    Player_Script player;
    private void Start()
    {
        player = FindObjectOfType<Player_Script>();
    }
    public void FieldAttack00_Motion()
    {
        switch(player.Skill_name)
        {
            case "일반마법":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("nomalMagic"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("nomalMagic", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("nomalMagic", false);
                break;
            case "바람의창":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("windSpear"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("windSpear", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("windSpear", false);
                break;
            case "돌무더기":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("stoneRain"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("stoneRain", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("stoneRain", false);
                break;
            case "불화살":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("FireArrow"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("FireArrow", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("FireArrow", false);
                break;
            case "전격":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("Lighting"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("Lighting", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("Lighting", false);
                break;
            case "고드름":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("iceBolt"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("iceBolt", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("iceBolt", false);
                break;

            case "절망의 균열":
                if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("abyssCrevice"))
                    AttackMotion_Object[0].GetComponent<Animator>().SetBool("abyssCrevice", true);
                else AttackMotion_Object[0].GetComponent<Animator>().SetBool("abyssCrevice", false);
                break;

            default:
                break;
        }
        player.Field_name = "";
        player.Skill_name = "";
    }
    public void FieldAttack01_Motion()
    {
        switch (player.Skill_name)
        {
            case "일반마법":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("nomalMagic"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("nomalMagic", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("nomalMagic", false);
                break;
            case "바람의창":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("windSpear"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("windSpear", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("windSpear", false);
                break;
            case "돌무더기":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("stoneRain"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("stoneRain", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("stoneRain", false);
                break;
            case "불화살":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("FireArrow"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("FireArrow", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("FireArrow", false);
                break;
            case "전격":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("Lighting"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("Lighting", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("Lighting", false);
                break;
            case "고드름":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("iceBolt"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("iceBolt", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("iceBolt", false);
                break;

            case "절망의 균열":
                if (!AttackMotion_Object[1].GetComponent<Animator>().GetBool("abyssCrevice"))
                    AttackMotion_Object[1].GetComponent<Animator>().SetBool("abyssCrevice", true);
                else AttackMotion_Object[1].GetComponent<Animator>().SetBool("abyssCrevice", false);
                break;

            default:
                break;
        }
        player.Field_name = "";
        player.Skill_name = "";
    }
    public void FieldAttack02_Motion()
    {
        switch (player.Skill_name)
        {
            case "일반마법":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("nomalMagic"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("nomalMagic", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("nomalMagic", false);
                break;
            case "바람의창":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("windSpear"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("windSpear", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("windSpear", false);
                break;
            case "돌무더기":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("stoneRain"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("stoneRain", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("stoneRain", false);
                break;
            case "불화살":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("FireArrow"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("FireArrow", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("FireArrow", false);
                break;
            case "전격":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("Lighting"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("Lighting", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("Lighting", false);
                break;
            case "고드름":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("iceBolt"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("iceBolt", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("iceBolt", false);
                break;

            case "절망의 균열":
                if (!AttackMotion_Object[2].GetComponent<Animator>().GetBool("abyssCrevice"))
                    AttackMotion_Object[2].GetComponent<Animator>().SetBool("abyssCrevice", true);
                else AttackMotion_Object[2].GetComponent<Animator>().SetBool("abyssCrevice", false);
                break;

            default:
                break;
        }
        player.Field_name = "";
        player.Skill_name = "";
    }
    public void FieldAttack03_Motion()
    {
        switch (player.Skill_name)
        {
            case "일반마법":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("nomalMagic"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("nomalMagic", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("nomalMagic", false);
                break;
            case "바람의창":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("windSpear"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("windSpear", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("windSpear", false);
                break;
            case "돌무더기":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("stoneRain"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("stoneRain", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("stoneRain", false);
                break;
            case "불화살":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("FireArrow"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("FireArrow", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("FireArrow", false);
                break;
            case "전격":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("Lighting"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("Lighting", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("Lighting", false);
                break;
            case "고드름":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("iceBolt"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("iceBolt", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("iceBolt", false);
                break;

            case "절망의 균열":
                if (!AttackMotion_Object[3].GetComponent<Animator>().GetBool("abyssCrevice"))
                    AttackMotion_Object[3].GetComponent<Animator>().SetBool("abyssCrevice", true);
                else AttackMotion_Object[3].GetComponent<Animator>().SetBool("abyssCrevice", false);
                break;

            default:
                break;
        }
        player.Field_name = "";
        player.Skill_name = "";
    }
    public void AllFieldAttack()
    {
        switch(player.Skill_name)
        {
            case "화염장판":
                for(int i = 0; i < 3; i++)
                {
                    if (!AttackMotion_Object[i].GetComponent<Animator>().GetBool("FireGround"))
                        AttackMotion_Object[i].GetComponent<Animator>().SetBool("FireGround", true);
                    else AttackMotion_Object[i].GetComponent<Animator>().SetBool("FireGround", false);
                }
                break;
            case "얼음안개":
                for(int i = 0; i < 3; i++)
                {
                    if (!AttackMotion_Object[i].GetComponent<Animator>().GetBool("iceFog"))
                        AttackMotion_Object[i].GetComponent<Animator>().SetBool("iceFog", true);
                    else AttackMotion_Object[i].GetComponent<Animator>().SetBool("iceFog", false);
                }
                break;

            default:
                break;
        }
        player.Field_name = "";
        player.Skill_name = "";
    }
}
