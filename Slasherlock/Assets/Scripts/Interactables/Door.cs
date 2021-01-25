using Assets.Scripts.Cameras;
using Assets.Scripts.Ui.Character;
using System;
using System.Collections;
using Assets.Scripts.Characters.Enemy;
using UnityEngine;

namespace Assets.Interactables.Physics
{
    public enum KeyColors
    {
        Golden,
        Blue,
        Green,
        Red,
        Cyan,
        Brown
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
        [SerializeField] bool unbrokable;
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
        [SerializeField] Sprite cyan;
        [SerializeField] Sprite brown;
        [SerializeField] SpriteRenderer keyLockRenderer;

        [SerializeField] float timeToUncorfimLock;
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

        float confirmLockedWhen;
        public bool NeedsKey => needsKey;
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
                KeyColors.Cyan => cyan,
                KeyColors.Brown => brown,
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
                if (CurrentState == State.Closed)
                    other.gameObject.GetComponent<EnemyFsm>().ResetAfterCloseDoorCoolDown();

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
                if (needsKey && !inventary.HasKey(keyName) && canInteract)
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
            confirmLockedWhen = Time.time;
            UpdatePath();
            CurrentState = State.ConfirmedLock;
        }

        void UnconfirmLockDoor()
        {
            if (CurrentState != State.ConfirmedLock) return;
            door.gameObject.layer = playerObstableLayer;
            confirmLockedWhen = 0;
            UpdatePath();
            CurrentState = State.Locked;
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

            if (CurrentState == State.ConfirmedLock
                && confirmLockedWhen > 0
                && (Time.time - confirmLockedWhen) > timeToUncorfimLock)
                UnconfirmLockDoor();


            audioSource.volume = Vector2.Distance(
                    transform.position,
                    inventary.transform.position) switch
                {
                    var dist when dist <= 15 => 1f,
                    var dist when dist <= 30 => .6f,
                    var dist when dist <= 50 => .4f,
                    _ => .2f,
                };
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
                else if (CurrentState == State.Open)
                {
                    CloseDoor();
                    LockDoor();
                }
                else if (IsDoorLocked())
                    UnlockDoor();
        }

        public void ForceOpen(bool skipUnbrokable)
        {
            if (unbrokable && !skipUnbrokable)
                return;

            hasLockIn = false;
            AnimOpenDoor();

            lockAnimator.Play("DropPadlock");
            CameraShaker.Instance.ShakeOnOpenDoor();

            IEnumerator wait()
            {
                var animationClip = lockAnimator.runtimeAnimatorController.animationClips[0];
                var oldPos = lockSimbol.transform.position;
                var oldScale = lockSimbol.transform.localScale;
                var oldRot = lockSimbol.transform.rotation;

                yield return new WaitForSeconds(animationClip.length);
                lockSimbol.SetActive(false);
                keyLockRenderer.enabled = false;
                lockSimbol.transform.position = oldPos;
                lockSimbol.transform.localScale = oldScale;
                lockSimbol.transform.rotation = oldRot;
            }

            StartCoroutine(wait());
            door.gameObject.layer = playerObstableLayer;
            audioSource.PlayOneShot(openForced);
            UpdatePath();
            CurrentState = State.Closed;
        }

        public bool IsUnbrokable => unbrokable;
    }
}