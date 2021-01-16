using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureFOV : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
