using UnityEngine;

namespace Assets.Scripts.SceneManagement
{
    public class LoadSceneOnCollision : MonoBehaviour
    {
        [SerializeField] string sceneToLoad;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
            {
                SceneLoader.Instance.LoadScene(sceneToLoad);
            }
        }
    }
}
