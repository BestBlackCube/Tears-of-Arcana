using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPaenl : MonoBehaviour
{
    [SerializeField] GameObject BlackScreen;
    // Start is called before the first frame update
    void Awake()
    {
        BlackScreen.SetActive(true);
    }
}
