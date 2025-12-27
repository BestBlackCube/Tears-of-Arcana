using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardInfoStatus_Script : MonoBehaviour
{
    CardDeckField_Script deckField;
    public TMP_Text CardName;
    public TMP_Text CardStatus;
    public GameObject CardImage;
    public Sprite ImageSet;
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
                if (CardName.text == "CardName") CardName.text = deckField.CardCode[i];
                if (CardStatus.text == "Int") CardName.text = deckField.CardCode[i];
                if (CardImage.GetComponent<SpriteRenderer>().sprite == null)
                    CardImage.GetComponent<SpriteRenderer>().sprite = ImageSet;
            }
        }
    }
}
