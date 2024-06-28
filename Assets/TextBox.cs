using System.Collections;
using System.Collections.Generic; // For Queue
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public GameObject textboxPanel;
    public GameObject SpeakerImage;
    public TMP_Text textboxText;
    public AudioSource typingSoundSource; // Assign in the inspector
    public AudioClip typingSound; // Assign in the inspector
    public float soundCooldown = 0.2f; // Time in seconds between each sound play
    private Queue<string> textQueue = new Queue<string>(); // Queue to store text messages
    private bool isTyping = false; // Flag to check if currently typing
    public GameObject DialoguePointer; // Reference to the arrow GameObject

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && textQueue.Count > 0)
        {
            StartCoroutine(TypeText(textQueue.Dequeue()));
            if (textQueue.Count == 0)
            {
                DialoguePointer.SetActive(false);
                StopCoroutine("AnimateDialoguePointer");
            }
            DialoguePointer.SetActive(false);
            StopCoroutine("AnimateDialoguePointer");
        }
    }

    public void ShowTextbox(string text)
    {
        textboxPanel.SetActive(true);
        SpeakerImage.SetActive(true);

        textQueue.Enqueue(text); // Enqueue the text message

        if (!isTyping) // Assuming animation starts when the first text is enqueued
        {
            StartCoroutine(TypeText(textQueue.Dequeue()));
        }
    }

    IEnumerator TypeText(string text)
{
    isTyping = true;
    textboxText.text = ""; // Start with an empty text
    float lastSoundTime = Time.time; // Initialize the last sound play time

    foreach (char c in text)
    {
        textboxText.text += c; // Add each character one by one
        if (c != ' ' && c!= ',' && c!= '♫' && typingSoundSource != null && typingSound != null && Time.time - lastSoundTime >= soundCooldown)
        {
            typingSoundSource.PlayOneShot(typingSound); // Play the typing sound
            lastSoundTime = Time.time;
        }
        yield return new WaitForSeconds(0.1f); // Wait a bit before adding the next character
    }

    yield return new WaitForSeconds(1f); // Wait a bit after finishing typing
    isTyping = false; // Finished typing the current message

    // Control the DialoguePointer based on the queue's state
    if (textQueue.Count > 0)
    {
        DialoguePointer.SetActive(true); // Show the DialoguePointer if there are more messages
        if (!DialoguePointer.activeSelf) // Check if the pointer is not already active
        {
            StopCoroutine("AnimateDialoguePointer"); // Ensure to stop the current animation if running
            StartCoroutine("AnimateDialoguePointer"); // Start the animation
        }
    }
    else
    {
        textboxText.text = "";
        HideTextbox();
    }
}

IEnumerator AnimateDialoguePointer()
{
    bool movingUp = true;
    float moveSpeed = 2f; // Speed of the arrow movement
    float moveAmount = 1f; // How much the arrow moves up and down
    Vector3 startPosition = DialoguePointer.transform.position;
    float threshold = 0.01f; // Threshold for position comparison

    while (DialoguePointer.activeSelf) // Loop while the DialoguePointer is active
    {
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPosition = movingUp ? startPosition + new Vector3(0, moveAmount, 0) : startPosition;
        DialoguePointer.transform.position = Vector3.MoveTowards(DialoguePointer.transform.position, targetPosition, step);

        // Check if the current position is close enough to the target position
        if (Vector3.Distance(DialoguePointer.transform.position, targetPosition) < threshold)
        {
            movingUp = !movingUp; // Change direction
        }
        yield return null;
    }
    }

    public void HideTextbox()
    {
        textboxPanel.SetActive(false);
        SpeakerImage.SetActive(false);
        DialoguePointer.SetActive(false); // Hide the arrow
        StopCoroutine("AnimateDialoguePointer"); // Stop animating the arrow
    }
}