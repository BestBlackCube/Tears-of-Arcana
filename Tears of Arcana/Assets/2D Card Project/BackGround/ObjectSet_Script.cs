using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSet_Script : MonoBehaviour
{
    //public float end_timer = 0f;
    CardDeckField_Script deckField;
    public bool Order_1 = false;
    public bool Order_2 = false;
    public bool Order_3 = false;
    public bool Order_4 = false;
    // Start is called before the first frame update
    void Start()
    {
        deckField = FindObjectOfType<CardDeckField_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
