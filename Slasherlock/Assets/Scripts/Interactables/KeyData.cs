using System;
using Assets.Interactables.Physics;
using UnityEngine;

public class KeyData : MonoBehaviour
{
    [SerializeField] KeyColors keyName;

    [SerializeField] Sprite golden;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite green;
    [SerializeField] Sprite red;
    [SerializeField] Sprite cyan;
    [SerializeField] Sprite brown;

    [SerializeField] SpriteRenderer keyRenderer;
    void Start()
    {
        keyRenderer.sprite = keyName switch
        {
            KeyColors.Golden => golden,
            KeyColors.Blue => blue,
            KeyColors.Green => green,
            KeyColors.Red => red,
            KeyColors.Cyan => cyan,
            KeyColors.Brown => brown,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public KeyColors KeyName => keyName;
}
