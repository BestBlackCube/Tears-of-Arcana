using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMotion_Script : MonoBehaviour
{
    public GameObject[] AttackMotion_Object;
    
    public void FieldAttack00_Motion()
    {
        if (!AttackMotion_Object[0].GetComponent<Animator>().GetBool("nomalMagic"))
            AttackMotion_Object[0].GetComponent<Animator>().SetBool("nomalMagic", true);
        else AttackMotion_Object[0].GetComponent<Animator>().SetBool("nomalMagic", false);
    }
    public void FieldAttack01_Motion()
    {

    }
    public void FieldAttack02_Motion()
    {

    }
    public void FieldAttack03_Motion()
    {

    }
}
