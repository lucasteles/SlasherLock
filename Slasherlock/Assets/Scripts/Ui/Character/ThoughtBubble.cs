using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui.Character
{
    public class ThoughtBubble : MonoBehaviour
    {
        public static ThoughtBubble Instance { get; private set; }

        [SerializeField] float widthByLetter;
        [SerializeField] RectTransform background;
        [SerializeField] TextMeshProUGUI thought;
        [SerializeField] float timePerLetter;
        [SerializeField] float timePerThought;
        Queue<string> thoughtQueue = new Queue<string>();
        string thoughtToShow = string.Empty;
        int currentLetter = 0;

        Coroutine coroutine;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            background.gameObject.SetActive(false);
        }

        public void ShowThought(string thought)
        {
            if (thoughtQueue.Contains(thought) && thoughtToShow == thought)
                return;

            if (thoughtToShow == string.Empty)
            {
                thoughtToShow = thought;
                SetBackgroundSize();
                coroutine = StartCoroutine(ShowThought());
            }
            else thoughtQueue.Enqueue(thought);
        }

        public void ShowThoughtUntil(string thoughtText, Func<bool> when)
        {
            if (string.IsNullOrWhiteSpace(thoughtText))
                return;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            thoughtQueue.Clear();
            background.gameObject.SetActive(false);
            currentLetter = 0;
            thought.text = thoughtToShow = string.Empty;
            thoughtToShow = thoughtText;
            SetBackgroundSize();
            coroutine = StartCoroutine(ShowThought(when));
        }

        void SetBackgroundSize()
        {
            background.gameObject.SetActive(true);
            background.localScale = new Vector3(widthByLetter * thoughtToShow.Length, background.localScale.y, 1);
        }

        IEnumerator ShowThought(Func<bool> until = null)
        {
            while (thoughtToShow != string.Empty && currentLetter < thoughtToShow.Length)
            {
                yield return new WaitForSeconds(timePerLetter);

                thought.text += thoughtToShow[currentLetter];
                currentLetter++;

                if (currentLetter >= thoughtToShow.Length)
                {
                    if (until != null)
                        yield return new WaitUntil(until);
                    else
                        yield return new WaitForSeconds(timePerThought);

                    currentLetter = 0;
                    thought.text = thoughtToShow = string.Empty;

                    if (thoughtQueue.Count > 0)
                    {
                        thoughtToShow = thoughtQueue.Dequeue();
                        SetBackgroundSize();
                    }
                    else
                    {
                        background.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}