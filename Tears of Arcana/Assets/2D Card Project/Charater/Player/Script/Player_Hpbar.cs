using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Hpbar : MonoBehaviour
{
    public GameObject canvas;
    public GameObject PlayerHpbar_prefab;
    RectTransform playerHpbar;
    Image nowHpbar;
    Player_Script player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Script>();
        playerHpbar = Instantiate(PlayerHpbar_prefab, canvas.transform).GetComponent<RectTransform>();

        Vector3 HpBarPos = new Vector3(transform.position.x - 21, transform.position.y + 4.5f, 1);
        playerHpbar.position = HpBarPos;

        nowHpbar = playerHpbar.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        nowHpbar.fillAmount = (float)player.nowHp / (float)player.maxHp;
    }
}
