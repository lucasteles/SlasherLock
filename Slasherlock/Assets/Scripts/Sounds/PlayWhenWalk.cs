using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayWhenWalk : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float minDistance;
    [SerializeField] float minMaxVolumeDistance;
    AudioSource audioSource;
    CheckWalking checkWalking;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        checkWalking = new CheckWalking(transform);    
    }

    void Update()
    {
        var playerDistance = (transform.position - player.position).magnitude;
        if (checkWalking.IsWaking() && playerDistance < minDistance)
        {
            if (playerDistance <= minMaxVolumeDistance)
                audioSource.volume = 1;
            else
                audioSource.volume = 1 - (playerDistance / minDistance);
            
            audioSource.PlayIfNotPlaying();
        }
        else if (checkWalking.IsStopped())
            audioSource.Stop();

        checkWalking.Update(Time.deltaTime);
    }
}