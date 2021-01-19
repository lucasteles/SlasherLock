using System.Collections.Generic;
using Assets.Interactables.Physics;
using Assets.Scripts.Ui.Character;
using UnityEngine;

public class CharacterInventary : MonoBehaviour
{
    ICollection<KeyColors> keys = new HashSet<KeyColors>();

    [SerializeField] LockInventoryUi lockInventoryUi;
    [SerializeField] KeysInventoryUi keysInventoryUi;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickSound;

    int numberOfLocks = 0;

    public bool HasKey(KeyColors keyName) => keys.Contains(keyName);

    public bool HasLocks() => numberOfLocks > 0;

    public void AddLock()
    {
        lockInventoryUi.AddLock();
        numberOfLocks++;
    }

    public void UseLock()
    {
        lockInventoryUi.RemoveLock();
        numberOfLocks--;
    }

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
            keysInventoryUi.AddKeyOfColor(key.KeyName);
            print($"Get key:{key.KeyName}");
            Destroy(key.gameObject);
            DoSound();
        }
        else if (other.gameObject.GetComponent<LockItem>() is { } item)
        {
            AddLock();
            print("Get lock");
            Destroy(item.gameObject);
            DoSound();
        }
    }
}
