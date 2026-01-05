using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnEnd_Script : MonoBehaviour
{
    public ObjectSet_Script Order;
    public EnemyObjectSet_Script ObjectSet;
    public bool endButton = false;
    public bool end_turn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.activeSelf)
        {
            end_turn = false;
        }
    }

    private void OnMouseOver()
    {
        endButton = true;
    }
    private void OnMouseExit()
    {
        endButton = false;
    }
    private void OnMouseDown()
    {
        if (endButton)
        {
            Order.Order = true;
            for(int i = 0; i < 5; i++)
            {
                if(ObjectSet.deckField.Card_inField != null)
                    ObjectSet.deckField.deckField_Reset(i);
            }
            GameObject.Find("BlackCavas").GetComponent<BlackScreen_Script>().blackScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
