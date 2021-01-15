using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Interactables.Physics;
using UnityEngine;

public class KeyData : MonoBehaviour
{
    [SerializeField] KeyNames keyName;
    public KeyNames KeyName => keyName;
}
