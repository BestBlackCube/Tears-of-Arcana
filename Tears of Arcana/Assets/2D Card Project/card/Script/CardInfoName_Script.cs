using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInfoName_Script : MonoBehaviour
{
    CardDeckField_Script deckField;
    public TMP_Text CardName;
    // Start is called before the first frame update
    void Start()
    {
        deckField = FindObjectOfType<CardDeckField_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (deckField.Card_inField[i] != null)
            {
                if(CardName.text == "CardName") CardName.text = deckField.CardCode[i];
            }
        }
    }
}
