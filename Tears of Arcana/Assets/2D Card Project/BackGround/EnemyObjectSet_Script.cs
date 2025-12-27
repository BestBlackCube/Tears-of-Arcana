using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectSet_Script : MonoBehaviour
{
    public GameObject[] Monster_Object;
    public GameObject[] Field_inMonster;
    public string[] Enemy_Name;
    public bool NextStage = false; 
    public int StageCount = 0;

    public Vector3[] Field_transform;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
        Field_transform[0] = new Vector3(4, 2, 5);
        Field_transform[1] = new Vector3(9, 2, 5);
        Field_transform[2] = new Vector3(15, 2, 5);
        Field_transform[3] = new Vector3(20, 2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        StageCount = PlayerPrefs.GetInt("Stage");
        if(NextStage)
        {
            StageChange(StageCount);
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

                Enemy_Name[0] = "Skeleton";
                Enemy_Name[1] = "Eye";
                Enemy_Name[2] = "Goblin";
                break;
            default:
                break;
        }
        NextStage = false;
    }
}
