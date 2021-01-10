using UnityEngine;

namespace Assets.Interactables
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
            Debug.Log(canOpen);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AstarPath.active.UpdateGraphs(door.GetComponent<BoxCollider2D>().bounds);
                door.SetActive(!door.activeSelf);
            }
        }
    }
}
