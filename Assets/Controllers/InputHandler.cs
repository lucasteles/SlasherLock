using Assets.Physics;
using UnityEngine;

namespace Assets.Controllers
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Mover mover;

        private void Update()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            mover.SetXInput(x);
            mover.SetYInput(y);
        }
    }
}
