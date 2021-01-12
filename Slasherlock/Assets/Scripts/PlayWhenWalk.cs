using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayWhenWalk : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float minDistance;
    [SerializeField] float minMaxVolumeDistance;
    
    AudioSource audioSource;
    Vector3 oldPosisition;
     
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        oldPosisition = transform.position;
    }

    void Update()
    {
        var playerDistance = (transform.position - player.position).magnitude;
        
        if (transform.position != oldPosisition && playerDistance < minDistance)
        {
            if (playerDistance <= minMaxVolumeDistance)
                audioSource.volume = 1;
            else
                audioSource.volume = 1 - (playerDistance / minDistance);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();

        oldPosisition = transform.position;
    }
}