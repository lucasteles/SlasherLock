using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerWheenSaw : MonoBehaviour
{
    public bool saw;
    private int defaultLayer;
    private int enemyLayer;

    public void Saw() => saw = true;

    void Start()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        gameObject.layer = defaultLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (saw)
        {
            gameObject.layer = enemyLayer;
            saw = false;
        }
        else
        {
            gameObject.layer = defaultLayer;
        }
    }
}