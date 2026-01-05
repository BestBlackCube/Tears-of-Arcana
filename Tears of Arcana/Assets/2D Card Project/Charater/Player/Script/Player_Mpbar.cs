using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Mpbar : MonoBehaviour
{
    public GameObject canvas;
    public GameObject PlayerMpbar_prefab;
    RectTransform playerMpbar;
    Image nowMpbar;
    Player_Script player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Script>();
        playerMpbar = Instantiate(PlayerMpbar_prefab, canvas.transform).GetComponent<RectTransform>();

        Vector3 HpBarPos = new Vector3(transform.position.x - 13, transform.position.y + 5f, 1);
        playerMpbar.position = HpBarPos;

        nowMpbar = playerMpbar.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        nowMpbar.fillAmount = (float)player.nowMp / (float)player.maxMp;
    }
}
