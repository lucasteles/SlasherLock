using Assets.Scripts.Ui.Character;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Interactables.Physics
{
    public enum KeyColors
    {
        Golden,
        Blue,
        Green,
        Red,
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

        public State GetState() => CurrentState;

        [SerializeField] bool needsKey;
        [SerializeField] KeyColors keyName = KeyColors.Golden;
        [SerializeField] AudioClip open;
        [SerializeField] AudioClip openForced;
        [SerializeField] AudioClip close;
        [SerializeField] AudioClip lockDoor;
        [SerializeField] AudioClip needLockPad;
        [SerializeField] AudioClip locked;


        [SerializeField] Sprite golden;
        [SerializeField] Sprite blue;
        [SerializeField] Sprite green;
        [SerializeField] Sprite red;
        [SerializeField] SpriteRenderer keyLockRenderer;

        Collider2D doorCollider;
        Animator animator;

        int obstableLayer;
        int playerObstableLayer;

        AudioSource audioSource;
        GameObject door;
        bool canInteract;
        bool hasSomeoneClose;
        State CurrentState = State.Closed;
        float timeToAutoClose = 0.8f;
        bool hasLockIn = false;
        bool enemyPlayLockSound = true;
        GameObject lockSimbol;
        CharacterInventary inventary;
        Animator lockAnimator;

        const string dontHaveLocksThought = "I don't have any locks...";
        const string dontHaveKeyThought = "I don't have the key...";

        public void Shake() => animator.SetTrigger("Shake");

        void Awake()
        {
            door = transform.GetChild(0).gameObject;
            doorCollider = door.GetComponent<Collider2D>();
            animator = door.GetComponent<Animator>();

            lockSimbol = transform.GetChild(1).gameObject;
            audioSource = GetComponent<AudioSource>();

            obstableLayer = LayerMask.NameToLayer("Obstacle");
            playerObstableLayer = LayerMask.NameToLayer("PlayerObstacle");


            lockAnimator = lockSimbol.GetComponent<Animator>();
        }

        void AnimOpenDoor()
        {
            animator.SetBool("Closed", false);
            doorCollider.enabled = false;
        }

        void AnimCloseDoor()
        {
            animator.SetBool("Closed", true);
            doorCollider.enabled = true;
        }

        void Start()
        {
            keyLockRenderer.sprite = keyName switch
            {
                KeyColors.Golden => golden,
                KeyColors.Blue => blue,
                KeyColors.Green => green,
                KeyColors.Red => red,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (needsKey)
                LockDoorKey();

            inventary = FindObjectOfType<CharacterInventary>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == door) return;
            hasSomeoneClose = true;

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
                if (CurrentState == State.Locked) ConfirmLockDoor();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == door) return;
            hasSomeoneClose = false;

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
            AnimCloseDoor();

            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.2f);
                audioSource.PlayOneShot(close);
            }

            StartCoroutine(wait());
            CurrentState = State.Closed;
        }

        void OpenDoor(bool playLockedSound = true)
        {
            if (CurrentState == State.Open) return;

            if (IsDoorLocked())
            {
                if (needsKey && !inventary.HasKey(keyName))
                    ThoughtBubble.Instance.ShowThought(dontHaveKeyThought);

                if (playLockedSound) audioSource.PlayOneShot(locked);
                return;
            }

            AnimOpenDoor();
            audioSource.PlayOneShot(open);
            door.gameObject.layer = playerObstableLayer;
            CurrentState = State.Open;
        }

        void LockDoor()
        {
            if (CurrentState == State.Locked) return;


            if (!inventary.HasLocks())
            {
                ThoughtBubble.Instance.ShowThought(dontHaveLocksThought);

                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(needLockPad);
                }

                return;
            }

            lockAnimator.Play("DropPadlock",-1, 0);
            lockSimbol.SetActive(true);
            inventary.UseLock();
            hasLockIn = true;
            audioSource.PlayOneShot(lockDoor);

            AnimCloseDoor();
            CurrentState = State.Locked;
        }

        void LockDoorKey()
        {
            if (CurrentState == State.Locked) return;
            keyLockRenderer.enabled = true;
            AnimCloseDoor();
            hasLockIn = false;
            CurrentState = State.Locked;
            ConfirmLockDoor();
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

            if (needsKey)
            {
                if (!inventary.HasKey(keyName))
                {
                    ThoughtBubble.Instance.ShowThought(dontHaveKeyThought);
                    audioSource.PlayOneShot(locked);
                    return;
                }

                inventary.AddLock();
                needsKey = false;
            }

            AnimCloseDoor();
            if (hasLockIn) inventary.AddLock();
            lockSimbol.SetActive(false);
            keyLockRenderer.enabled = false;
            door.gameObject.layer = playerObstableLayer;
            audioSource.PlayOneShot(lockDoor);
            UpdatePath();
            CurrentState = State.Closed;
        }

        void UpdatePath() => //AstarPath.active.Scan();
            AstarPath.active.UpdateGraphs(GetComponent<BoxCollider2D>().bounds);

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

            if (Input.GetKeyDown(KeyCode.Q))
                if (CurrentState == State.Closed)
                    LockDoor();
                else if (IsDoorLocked())
                    UnlockDoor();
        }

        public void ForceOpen()
        {
            hasLockIn = false;
            AnimOpenDoor();

            lockAnimator.Play("DropPadlock");

            IEnumerator wait()
            {
                var animationClip = lockAnimator.runtimeAnimatorController.animationClips[0];
                yield return new WaitForSeconds(animationClip.length);
                lockSimbol.SetActive(false);
                keyLockRenderer.enabled = false;
            }

            StartCoroutine(wait());
            door.gameObject.layer = playerObstableLayer;
            audioSource.PlayOneShot(openForced);
            UpdatePath();
            CurrentState = State.Closed;
        }
    }
}