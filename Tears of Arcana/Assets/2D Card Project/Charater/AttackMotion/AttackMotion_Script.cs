using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion_Script : MonoBehaviour
{
    [SerializeField] GameObject Attack_Object;
    void Start()
    {
        Attack_Object = GameObject.Find("Public_AttackObject");
    }
    void PublicAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("PublicAttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion01", false);
    }
    void PublicAttackMotion02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("PublicAttackMotion02"))
            Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion02", false);
    }
    void PublicAttackMotion03()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("PublicAttackMotion03"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion03", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("PublicAttackMotion03", false);
        }
    }

    void SlashAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion01"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion01", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion01", false);
        }
    }
    void SlashAttackMotion01_02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion01-02"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion01-02", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion01-02", false);
        }
    }
    void SlashAttackMotion02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion02"))
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02", false);
    }
    void SlashAttackMotion02_02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion02-02"))
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02-02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02-02", false);
    }
    void SlashAttackMotion02_03()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion02-03"))
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02-03", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion02-03", false);
    }
    void SlashAttackMotion03()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion03"))
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion03", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion03", false);
    }
    void SlashAttackMotion03_02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion03-02"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion03-02", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion03-02", false);
        }
    }
    void SlashAttackMotion04()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Slash_AttackMotion04"))
            Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion04", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Slash_AttackMotion04", false);
    }
    void FireBrassAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("FireBrass_AttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("FireBrass_AttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("FireBrass_AttackMotion01", false);
    }
    void ExplosionAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Explosion_AttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01", false);
    }
    void ExplosionAttackMotion01_02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Explosion_AttackMotion01-02"))
            Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-02", false);
    }
    void ExplosionAttackMotion01_03()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Explosion_AttackMotion01-03"))
            Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-03", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-03", false);
    }
    void ExplosionAttackMotion01_04()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Explosion_AttackMotion01-04"))
            Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-04", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-04", false);
    }
    void ExplosionAttackMotion01_05()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Explosion_AttackMotion01-05"))
            Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-05", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Explosion_AttackMotion01-05", false);
    }
    void BulletAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Bullet_AttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("Bullet_AttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("Bullet_AttackMotion01", false);
    }
    void RumblingAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("Rumbling_AttackMotion01"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Rumbling_AttackMotion01", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(0, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("Rumbling_AttackMotion01", false);
        }
    }

    //BOSS

    void ArgonAttackMotion1()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("ArgonAttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("ArgonAttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("ArgonAttackMotion01", false);
    }
    void ArgonAttackMotion2()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("ArgonAttackMotion02"))
            Attack_Object.GetComponent<Animator>().SetBool("ArgonAttackMotion02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("ArgonAttackMotion02", false);
    }
    void TitanAttackMotion1()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("TitanAttackMotion01"))
        {
            Attack_Object.transform.localScale = new Vector3(-1, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("TitanAttackMotion01", true);
        }
        else
        {
            Attack_Object.transform.localScale = new Vector3(0, Attack_Object.transform.position.y, Attack_Object.transform.position.z);
            Attack_Object.GetComponent<Animator>().SetBool("TitanAttackMotion01", false);
        }
    }
    void CastleGuardianAttackMotion01()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("CastleGuardianAttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion01", false);
    }
    void CastleGuardianAttackMotion02()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("CastleGuardianAttackMotion02"))
            Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion02", true);
        else Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion02", false);
    }
    void CastleGuardianAttackMotion03()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("CastleGuardianAttackMotion03"))
            Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion03", true);
        else Attack_Object.GetComponent<Animator>().SetBool("CastleGuardianAttackMotion03", false);
    }
    void DemonSlimeAttackMotion1()
    {
        if (!Attack_Object.GetComponent<Animator>().GetBool("DemonSlimeAttackMotion01"))
            Attack_Object.GetComponent<Animator>().SetBool("DemonSlimeAttackMotion01", true);
        else Attack_Object.GetComponent<Animator>().SetBool("DemonSlimeAttackMotion01", false);
    }
}
