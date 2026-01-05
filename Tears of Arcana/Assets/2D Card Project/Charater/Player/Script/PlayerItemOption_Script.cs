using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemOption_Script : MonoBehaviour
{
    [SerializeField] Item_Carddata itemName;
    [SerializeField] card_Status item;
    [SerializeField] Player_Script player;

    public string ObjectName;
    public string CardName;

    public bool OptionOn = false;
    public bool OptionActive = false;

    public Vector3 baseTrasnform = new Vector3(19.3f, -6.2f, 5);
    // Start is called before the first frame update
    void Start()
    {
        item = new card_Status();
        item = item.item_Card(itemName);
        CardName = item.InputName;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && OptionActive)
        {
            this.transform.position = baseTrasnform;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            if (ObjectName != "") ObjectName = "";
            OptionActive = false;
            OptionOn = false;
            Player_target(false);
        }
        if (OptionActive) Card_Mouse();
    }
    private void OnMouseOver()
    {
        OptionOn = true;
        this.transform.position = new Vector3(19.3f, -4.2f, 6);
    }
    private void OnMouseExit()
    {
        OptionOn = false;
        this.transform.position = new Vector3(19.3f, -6.2f, 6);
    }
    private void OnMouseDown()
    {
        if(OptionOn)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 4);
            OptionActive = true;
            Player_target(true);
        }
    }
    void Card_Mouse()
    {
        switch(ObjectName)
        {
            case "Player":
                Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 6, 0);
                transform.position = Vector3.Lerp(transform.position, playerPosition, 5 * Time.deltaTime);
                break;
            default:
                Vector3 worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(worldPosition);
                transform.position = Vector3.Lerp(transform.position, mousePosition, 10 * Time.deltaTime);
                break;
        }
    }

    public void Player_target(bool Click)
    {
        if(Click) player.Arrow = true;
        else player.Arrow = false;
    }
}
