using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBehindMask : MonoBehaviour
{
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("BehindMask");
    }
}
