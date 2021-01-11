using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class UpdateFieldOfView : MonoBehaviour
    {
        [SerializeField] FieldOfView fieldOfView;

        private void Update()
        {
            fieldOfView.SetOrigin(transform.position);
        }
    }
}
