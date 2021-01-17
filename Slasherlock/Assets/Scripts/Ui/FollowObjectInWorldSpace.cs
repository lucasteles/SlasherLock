using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class FollowObjectInWorldSpace : MonoBehaviour 
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 distance;

        private void Update()
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position) + distance;
        }
    }
}
