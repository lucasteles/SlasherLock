using UnityEngine;

namespace Assets.Interactables.Physics
{
    public class Door : MonoBehaviour
    {
        [SerializeField]AudioClip open;
        [SerializeField]AudioClip close;
        AudioSource audioSource;
        GameObject door;
        bool canOpen;

        float timeToAutoClose = 0.5f;
        float autoCloseTimer = 0f;

        private void Awake()
        {
            door = transform.GetChild(0).gameObject;
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canOpen = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            canOpen = false;
            Invoke(nameof(AutoCloseDoor), timeToAutoClose);
        }

        void AutoCloseDoor()
        {
            if (!canOpen)
                CloseDoor();
        }

        void CloseDoor()
        {
            if (!door.activeSelf)
            {
                door.SetActive(true);
                audioSource.PlayOneShot(close);
                UpdatePath();
            }
        }

        void OpenDoor()
        {
            if (door.activeSelf)
            {
                door.SetActive(false);
                audioSource.PlayOneShot(open);
                UpdatePath();
            }
        }

        void UpdatePath() => AstarPath.active.UpdateGraphs(door.GetComponent<BoxCollider2D>().bounds);
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || !canOpen) return;

            if (door.activeSelf)
                OpenDoor();
            else
                CloseDoor();

        }
    }
}
