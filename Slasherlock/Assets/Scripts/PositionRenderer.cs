using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderer : MonoBehaviour
{
    int SortingOrderBase = 50000;
    [SerializeField] int Offset;
    Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.sortingOrder = (int) (SortingOrderBase - transform.position.y - Offset);
    }
}
