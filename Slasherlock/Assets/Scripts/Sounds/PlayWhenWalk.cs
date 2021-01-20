using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class PlayWhenWalk : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float minDistance;
    [SerializeField] float minMaxVolumeDistance;

    [SerializeField] float minGrain;
    [SerializeField] float maxGrain;
    [SerializeField] float currentGrain;

    AudioSource audioSource;
    CheckWalking checkWalking;

    FilmGrain grain;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        var volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out grain);
        grain.intensity.value = minGrain;
    }

    void Start()
    {
        checkWalking = new CheckWalking(transform);
    }

    void Update()
    {
        var playerDistance = (transform.position - player.position).magnitude;

        if (grain != null)
            if (playerDistance < minDistance )
            {
                var newGrain = (playerDistance * (maxGrain - minGrain)) / (minDistance );
                grain.intensity.value = maxGrain - newGrain ;// Mathf.Clamp(newGrain, minGrain, maxGrain);
                currentGrain = grain.intensity.value;
            }
            else
                grain.intensity.value = minGrain;

        if (checkWalking.IsWaking() && playerDistance < minDistance)
        {
            if (playerDistance <= minMaxVolumeDistance)
                audioSource.volume = 1;
            else
                audioSource.volume = 1 - (playerDistance / minDistance);

            audioSource.PlayIfNotPlaying();
        }
        else if (checkWalking.IsStoppedAndReset())
            audioSource.Stop();

        checkWalking.Update(Time.deltaTime);
    }
}