using System;
using UnityEngine;

namespace Assets.Interactables.Physics
{
    public enum KeyNames
    {
        NoKey,
        LeftDoor,
    }

    public class Door : MonoBehaviour
    {
        public enum State
        {
            Open,
            Closed,
            Locked,
            ConfirmedLock,
        }

        [SerializeField] KeyNames keyName = KeyNames.NoKey;
        [SerializeField] AudioClip open;
        [SerializeField] AudioClip close;
        [SerializeField] AudioClip lockDoor;
        [SerializeField] AudioClip locked;

        int obstableLayer;
        int playerObstableLayer;

        AudioSource audioSource;
        GameObject door;
        bool canInteract;
        bool hasSomeoneClose;
        State CurrentState = State.Closed;
        float timeToAutoClose = 0.8f;

        bool enemyPlayLockSound = true;
        SpriteRenderer lockSimbol;
        SpriteRenderer lockKeySimbol;
        CharacterInventary	inventary;

        void Awake()
        {
            door = transform.GetChild(0).gameObject;
            lockSimbol = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            lockKeySimbol = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();

            obstableLayer = LayerMask.NameToLayer("Obstacle");
            playerObstableLayer = LayerMask.NameToLayer("PlayerObstacle");
        }

        void Start()
        {
            if (keyName != KeyNames.NoKey)
            {
                LockDoor(withKey: true);
                ConfirmLockDoor();
            }
            inventary = FindObjectOfType<CharacterInventary>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == door) return;
            hasSomeoneClose = true;
            // print("Entrou: " + collision.gameObject.name);
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
                canInteract = true;
        }

        void EnableEnemyToPlayLockedSound() => enemyPlayLockSound = true;

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject == door) return;
            hasSomeoneClose = true;
            if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
            {
                OpenDoor(enemyPlayLockSound);
                if (enemyPlayLockSound)
                    Invoke(nameof(EnableEnemyToPlayLockedSound), 1f);
                enemyPlayLockSound = false;
            }

        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == door) return;
            hasSomeoneClose = false;

            // print("Saiu: " + other.gameObject.name);
            if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
                canInteract = false;
            Invoke(nameof(AutoCloseDoor), timeToAutoClose);
        }

        bool IsDoorLocked() => CurrentState == State.Locked || CurrentState == State.ConfirmedLock;
        void AutoCloseDoor()
        {
            if (!hasSomeoneClose)
                CloseDoor();
        }

        void CloseDoor()
        {
            if (CurrentState == State.Closed || IsDoorLocked()) return;
            door.SetActive(true);
            audioSource.PlayOneShot(close);
            CurrentState = State.Closed;
        }

        void OpenDoor(bool playLockedSound = true)
        {
            if (CurrentState == State.Open) return;

            if (IsDoorLocked())
            {
                if (playLockedSound) audioSource.PlayOneShot(locked);
                if (CurrentState == State.Locked) ConfirmLockDoor();
                return;
            }

            door.SetActive(false);
            audioSource.PlayOneShot(open);
            door.gameObject.layer = playerObstableLayer;
            CurrentState = State.Open;
        }

        void LockDoor(bool withKey = false)
        {
            if (CurrentState == State.Locked) return;
            door.SetActive(true);
            (withKey ? lockKeySimbol : lockSimbol).enabled = true;
            if (!withKey)
                audioSource.PlayOneShot(lockDoor);
            CurrentState = State.Locked;
        }
        void ConfirmLockDoor()
        {
            if (CurrentState == State.ConfirmedLock) return;
            door.gameObject.layer = obstableLayer;
            UpdatePath();
            CurrentState = State.ConfirmedLock;
        }

        void UnlockDoor()
        {
            if (!IsDoorLocked()) return;

            if (keyName != KeyNames.NoKey)
            {
                if (!inventary.HasKey(keyName))
                {
                    audioSource.PlayOneShot(locked);
                    return;
                }

                inventary.Consume(keyName);
                keyName = KeyNames.NoKey;
            }

            door.SetActive(true);
            lockSimbol.enabled = lockKeySimbol.enabled = false;
            door.gameObject.layer = playerObstableLayer;
            audioSource.PlayOneShot(lockDoor);
            UpdatePath();
            CurrentState = State.Closed;
        }

        void UpdatePath() => AstarPath.active.UpdateGraphs(door.GetComponent<BoxCollider2D>().bounds);

        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (!canInteract) return;

            if (Input.GetKeyDown(KeyCode.Space))
                if (CurrentState != State.Open)
                    OpenDoor();
                else
                    CloseDoor();

            if (Input.GetKeyDown(KeyCode.Return))
                if (CurrentState == State.Closed)
                    LockDoor();
                else if (IsDoorLocked())
                    UnlockDoor();
        }
    }
}