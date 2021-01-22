using System;
using UnityEngine;

public class WalkFlag : MonoBehaviour
{
    [SerializeField]float maxTimeToReset = 1;
    GameObject player;

    public float distanceFromPlayer;
    public bool isVisible;
    float lastVisibleTime;

    public void SetVisibleByPlayer()
    {
        lastVisibleTime = 0;
        isVisible = true;
    }

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        player = FindObjectOfType<CharacterInventary>().gameObject;
    }

    void Update()
    {
        if (isVisible)
        {
            lastVisibleTime += Time.deltaTime;
            if (lastVisibleTime >= maxTimeToReset)
                isVisible = false;
        }

        distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
    }

    public void Show() => GetComponent<Renderer>().enabled = true;
    public void Hide() => GetComponent<Renderer>().enabled = false;
}