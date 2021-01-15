using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Interactables.Physics;
using UnityEngine;

public class CharacterInventary : MonoBehaviour
{
    ICollection<KeyNames> keys = new HashSet<KeyNames>();

    // TODO: lidar com quantidade de cadeados
    int numberOfLocks = 1;

    public bool HasKey(KeyNames keyName) => keys.Contains(keyName);
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<KeyData>() is {} key)
        {
            if (key.KeyName == KeyNames.NoKey)
                return;

            keys.Add(key.KeyName);
            print($"Get key:{key.KeyName}");
            Destroy(key.gameObject);
        }

    }

    public void Consume(KeyNames keyName)
    {
        keys.Remove(keyName);
    }
}
