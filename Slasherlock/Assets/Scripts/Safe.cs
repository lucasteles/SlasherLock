using System.Collections;
using System.Linq;
using Assets.Interactables.Physics;
using Assets.Scripts.Ui;
using Assets.Scripts.Ui.Character;
using UnityEngine;
using Random = UnityEngine.Random;

public class Safe : MonoBehaviour
{
    bool canInteract;

    [SerializeField] public string name;
    [SerializeField] GameObject keyPefab;
    [SerializeField] KeyColors keyColor;
    [SerializeField] SafePasswordUi safePasswordUi;

    AudioSource source;

    [SerializeField] string password;
    public string Password => password;
    string message => $"Safe: {name}";

    void Awake()
    {
        safePasswordUi.gameObject.SetActive(false);
        GeneratePassword();
        source = GetComponent<AudioSource>();
    }

    void GeneratePassword()
    {
        var opcoes = "123456789";
        password = Enumerable
            .Range(0, 4)
            .Select(_ => opcoes[Random.Range(0, opcoes.Length)].ToString())
            .Aggregate(string.Concat);
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
        {
            canInteract = false;
            safePasswordUi.ResetPassword();
            safePasswordUi.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.Q) && !safePasswordUi.gameObject.activeSelf)
            TryOpenSafe();
        else if (canInteract && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q)))
        {
            safePasswordUi.gameObject.SetActive(false);
            safePasswordUi.ResetPassword();
        }
    }

    void TryOpenSafe()
    {
        safePasswordUi.gameObject.SetActive(true);
    }

    public bool TestPassword(string password)
    {
        if (Password == password)
        {
            OpenSafe();
            safePasswordUi.gameObject.SetActive(false);
            safePasswordUi.ResetPassword();
            return true;
        }

        return false;
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