using UnityEngine;

namespace Assets.Interactables.Physics
{
    public class Door : MonoBehaviour
    {
        GameObject door;
        bool canOpen;

        private void Awake()
        {
            door = transform.GetChild(0).gameObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canOpen = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            canOpen = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AstarPath.active.UpdateGraphs(door.GetComponent<BoxCollider2D>().bounds);
                door.SetActive(!door.activeSelf);
            }
        }
    }
}
