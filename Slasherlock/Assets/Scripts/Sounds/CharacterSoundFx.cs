using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFx : MonoBehaviour
{
    [SerializeField]AudioSource runAudioSource;
    [SerializeField]AudioSource breathAudioSource;
    [SerializeField]float secondsToBreath;

    float passedTime = 0;
    CheckWalking checkWalking;

    void Start()
    {
        checkWalking = new CheckWalking(transform);    
    }

    void Update()
    {
        if (checkWalking.IsWaking())
        {
            passedTime += Time.deltaTime;
            runAudioSource.PlayIfNotPlaying();
        }
        else if (checkWalking.IsStoppedAndReset())
        {
            passedTime = 0;
            runAudioSource.Stop();
        }


        checkWalking.Update(Time.deltaTime);
        if (passedTime > secondsToBreath)
            breathAudioSource.PlayIfNotPlaying();
    }
}
