using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }

}
