using System.Collections;
using Assets.Interactables.Physics;
using Assets.Scripts.Ui.Character;
using UnityEngine;
using Random = UnityEngine.Random;

public class Safe : MonoBehaviour
{
    bool canInteract;

    [SerializeField] public string message;
    [SerializeField] GameObject keyPefab;
    [SerializeField] KeyColors keyColor;
    [SerializeField] AudioClip beep;

    AudioSource source;

    string password;
    public string Password => password;

    void Awake()
    {
        GeneratePassword();
        source = GetComponent<AudioSource>();
    }

    void GeneratePassword()
    {
        password = Random.Range(0, 9999).ToString("D4");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            ThoughtBubble.Instance.ShowThought(message);
            canInteract = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            ThoughtBubble.Instance.ShowThought(message);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            canInteract = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.Q))
            TryOpenSafe();
    }

    void TryOpenSafe()
    {
        OpenSafe();
    }

    void OpenSafe()
    {
        source.Play();

        IEnumerator wait()
        {
            yield return new WaitForSeconds(1);
            Instantiate(keyPefab, transform.position, Quaternion.identity).GetComponent<KeyData>()
                .SetKeyColor(keyColor);
            GetComponentInChildren<Renderer>().enabled = false;
            yield return new WaitUntil(() => !source.isPlaying);
            Destroy(gameObject);
        }

        StartCoroutine(wait());
    }
}