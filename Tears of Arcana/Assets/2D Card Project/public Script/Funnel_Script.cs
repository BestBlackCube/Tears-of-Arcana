using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funnel_Script : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
