using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetBar_Script : MonoBehaviour
{
    public GameObject[] guideTarget;
    public Transform target;
    public Vector3[] offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
            for(int i = 0;  i < 4; i ++)
            {
                guideTarget[i].transform.position = target.position + offset[i];
            }
        }
    }
}
