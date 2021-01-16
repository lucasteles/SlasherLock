using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLobo : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float eachSeconds;
    [SerializeField] float percentOf;

    float passedTime;

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime > eachSeconds)
        {
            passedTime = 0;
            if (Random.value <= percentOf)
                audioSource.Play();
        }
    }
}
