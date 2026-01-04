using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyObjectSet_Script : MonoBehaviour
{
    public CardDeck_Script deck;
    public GameObject[] Monster_Object;
    public GameObject[] Field_inMonster;
    public string[] Enemy_Name;
    public bool NextStage = false;
    public bool CardAdd = false;
    public int StageCount = 0;

    public Vector3[] Field_transform;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
        Field_transform[0] = new Vector3(-5, 2, 5);
        Field_transform[1] = new Vector3(1, 2, 5);
        Field_transform[2] = new Vector3(7, 2, 5);
        Field_transform[3] = new Vector3(13, 2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        StageCount = PlayerPrefs.GetInt("Stage");
        if(NextStage)
        {
            StageChange(StageCount);
        }
        if(CardAdd)
        {
            deck.Cardinput = true;
            CardAdd = false;
        }
    }
    void StageChange(int Stage)
    {
        switch(Stage)
        {
            case 1:
                Field_inMonster[0] = Instantiate(Monster_Object[0], Field_transform[0], Quaternion.identity);
                Field_inMonster[1] = Instantiate(Monster_Object[1], Field_transform[1], Quaternion.identity);
                Field_inMonster[2] = Instantiate(Monster_Object[2], Field_transform[2], Quaternion.identity);
                Field_inMonster[3] = Instantiate(Monster_Object[3], Field_transform[3], Quaternion.identity);

                Enemy_Name[0] = "Skeleton";
                Enemy_Name[1] = "Eye";
                Enemy_Name[2] = "Goblin";
                Enemy_Name[3] = "Mushroom";
                break;
            default:
                break;
        }
        NextStage = false;
    }
}
