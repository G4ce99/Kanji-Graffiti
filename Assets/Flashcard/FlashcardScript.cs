using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashcardController : MonoBehaviour
{
    public Image flashcardImage; // Assign the UI Image in the Inspector
    public Sprite[] flashcards; // Assign flashcard sprites in the Inspector
    private int currentCardIndex = 0;

    public void ShowNextCard()
    {
        currentCardIndex = (currentCardIndex + 1) % flashcards.Length;
        UpdateFlashcard();
    }

    public void ShowPreviousCard()
    {
        currentCardIndex = (currentCardIndex - 1 + flashcards.Length) % flashcards.Length;
        UpdateFlashcard();
    }

    private void UpdateFlashcard()
    {
        flashcardImage.sprite = flashcards[currentCardIndex];
    }
}