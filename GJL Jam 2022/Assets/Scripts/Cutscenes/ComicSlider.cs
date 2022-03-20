using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ComicSlider : MonoBehaviour
{
    [Tooltip("What gamepad/keyboard button action ID should trigger the next card?")]
    public string advanceButton = "Jump";

    [Tooltip("Which image should be in front, starting from zero?")]
    public int cardToShow = 0;

    [Tooltip("How fast to flip cards (in cards per second)")]
    public float flipSpeed = 5f;

    [Tooltip("How many degrees should each card behind the top card rotate?")]
    public float rotationIncrement = -5f;

    [Tooltip("How far should the next card be offset from the top card position?")]
    public Vector2 fanIncrement = new Vector2(15, -15);

    [Tooltip("Where should the top card fly to when we skip past it?")]
    public Vector2 flipAwayOffset = new Vector2(-100, 0);

    [Tooltip("What should happen when we're left with an empty stack?")]
    public UnityEvent OnFinishedStack;

    [SerializeField] private int[] _numberOfContinues;

    private Image[] _images;
    private Vector2 _centerPosition;
    private float _currentCard;
    private bool _hasFinished;
    private DialogueManager _dialogueManager;


    private void OnEnable()
    {
        DialogueManager.OnSetTransition += NextSlide;
    }
    private void OnDisable()
    {
        DialogueManager.OnSetTransition -= NextSlide;
    }

    // Move to next card
    public bool TryAdvance()
    {
        if (cardToShow >= _images.Length)
            return false;
        cardToShow++;
        return true;
    }

    public void NextSlide()
    {
            TryAdvance();
    }

    void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _images = GetComponentsInChildren<Image>();
        Array.Reverse(_images);

        // Remember where the lead image is.
        _centerPosition = _images[0].rectTransform.anchoredPosition;

        // Update the display of the rest of the stack.
        Layout();
    }

    // Check input, advance/animate if needed.
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TryAdvance();
        // }

        if (Mathf.Approximately(cardToShow, _currentCard))
        {
            if (cardToShow == _images.Length && !_hasFinished)
            {
                _hasFinished = true;
                Debug.Log("Finished stack!");

                if (OnFinishedStack != null)
                    OnFinishedStack.Invoke();
            }

            return;
        }

        _currentCard = Mathf.MoveTowards(_currentCard, cardToShow, flipSpeed * Time.deltaTime);
        Layout();
    }

    // Update cards
    void Layout()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            var image = _images[i];

            float t = i - _currentCard;

            var color = image.color;

            //if (t == 0) color.a = Mathf.Clamp01(t + 1f);
            //else color.a = 0;
            
            color.a = Mathf.Clamp01(t + 1f);
            image.color = color;

            var trans = image.rectTransform;
            trans.localRotation = Quaternion.Euler(0, 0, rotationIncrement * t);

            trans.anchoredPosition = _centerPosition + (t < 0f
                ? Vector2.Lerp(flipAwayOffset, Vector2.zero, t + 1f)
                : Mathf.Pow(t, 0.75f) * fanIncrement);
        }
    }
}