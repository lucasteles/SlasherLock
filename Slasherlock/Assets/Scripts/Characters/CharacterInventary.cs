using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Interactables.Physics;
using UnityEngine;

public class CharacterInventary : MonoBehaviour
{
    ICollection<KeyColors> keys = new HashSet<KeyColors>();

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickSound;

    int numberOfLocks = 0;

    public bool HasKey(KeyColors keyName) => keys.Contains(keyName);

    public bool HasLocks() => numberOfLocks > 0;
    public void AddLock() => numberOfLocks++;
    public void UseLock() => numberOfLocks--;

    void DoSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(pickSound);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<KeyData>() is {} key)
        {
            keys.Add(key.KeyName);
            print($"Get key:{key.KeyName}");
            Destroy(key.gameObject);
            DoSound();
        }
        else if (other.gameObject.GetComponent<LockItem>() is { } item)
        {
            numberOfLocks++;
            print("Get lock");
            Destroy(item.gameObject);
            DoSound();
        }

    }

}
