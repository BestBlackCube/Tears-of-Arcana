using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSet_Script : MonoBehaviour
{
    //public float end_timer = 0f;
    public EnemyObjectSet_Script ObjectSet;
    public CardDeck_Script deck;
    public bool Order_1 = false;
    public bool Order_2 = false;
    public bool Order_3 = false;
    public bool Order_4 = false;
    public bool CardAdd = false;
    public bool Order = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CardAdd)
        {
            deck.Cardinput = true;
            CardAdd = false;
        }
        if (Order)
        {
            if (ObjectSet.Field_inMonster[0] != null && ObjectSet.Field_inMonster[1] != null && 
                ObjectSet.Field_inMonster[2] != null && ObjectSet.Field_inMonster[3] != null)
            {
                Order_1 = true;
                Order = false;
            }

            if (ObjectSet.Field_inMonster[0] != null)
            {
                Order_1 = true;
                Order = false;
            }
            else if (ObjectSet.Field_inMonster[1] != null)
            {
                Order_2 = true;
                Order = false;
            }
            else if (ObjectSet.Field_inMonster[2] != null)
            {
                Order_3 = true;
                Order = false;
            }
            else if (ObjectSet.Field_inMonster[3] != null)
            {
                Order_4 = true;
                GameObject.Find("BlackCavas").GetComponent<BlackScreen_Script>().blackScreen.gameObject.SetActive(false);
                Order = false;
            }
            else if (ObjectSet.Enemy_Name[0] == null && ObjectSet.Enemy_Name[1] == null &&
                     ObjectSet.Enemy_Name[2] == null && ObjectSet.Enemy_Name[3] == null)
            {
                CardAdd = true;
                Order = false;
            }
        }
    }
}
