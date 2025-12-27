using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbar : MonoBehaviour
{
    public GameObject HpBar;
    GameObject hpbar;

    public Charater_namedata unitname;
    public Charater_Status status;

    public int nowHp;
    public int maxHp;

    Image EnemyHP;

    // Start is called before the first frame update
    void Start()
    {
        status = new Charater_Status();
        status = status.Char_inStatus(unitname);
        nowHp = status.NowHp;
        maxHp = status.MaxHp;

        Vector3 skeletonOffset = new Vector3(1, 1, 0);
        hpbar = Instantiate(HpBar, skeletonOffset, Quaternion.identity);
        EnemyHP = hpbar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHP.fillAmount = (float)nowHp / (float)maxHp;
    }
}
