using UnityEngine;

namespace Assets.Scripts.Fov
{
    public class MoveFieldOfView : MonoBehaviour
    {
        [SerializeField] FieldOfView fieldOfView;

        private void Update()
        {
            fieldOfView.SetOrigin(transform.position);
        }
    }
}
