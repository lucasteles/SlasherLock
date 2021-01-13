using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFx : MonoBehaviour
{
    [SerializeField]AudioSource runAudioSource;
    [SerializeField]AudioSource breathAudioSource;
    [SerializeField]float secondsToBreath;

    float maxRestTime = 0.1f;

    Rigidbody2D rb;
    float passedTime = 0;
    float restTime = 0;
    CheckWalking checkWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkWalking = new CheckWalking(transform);    
    }

    void Update()
    {
        if (checkWalking.IsWaking())
        {
            restTime = 0;
            passedTime += Time.deltaTime;
            runAudioSource.PlayIfNotPlaying();
        }
        else
        {
            if (restTime >= maxRestTime)
            {
                passedTime = 0;
                restTime = 0;
                runAudioSource.Stop();
            }

            restTime += Time.deltaTime;
        }

        checkWalking.Update();
        if (passedTime > secondsToBreath)
            breathAudioSource.PlayIfNotPlaying();
    }
}
