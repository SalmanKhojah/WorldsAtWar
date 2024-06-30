using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OldManTextBox : MonoBehaviour
{
    public GameObject textboxPanel;
    //public GameObject SpeakerImage;
    public TMP_Text textboxText;
    public AudioSource typingSoundSource;
    public AudioClip typingSound;
    public float soundCooldown = 0.2f;
    private Queue<string> textQueue = new Queue<string>();
    private bool isTyping = false;
    public GameObject DialoguePointer;

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
        //SpeakerImage.SetActive(true);

        textQueue.Enqueue(text);

        if (!isTyping)
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
        if (c != ' ' && typingSoundSource != null && typingSound != null && Time.time - lastSoundTime >= soundCooldown)
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
            StartCoroutine("AnimateDialoguePointer"); // Start the animation
        
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
        float moveSpeed = 2f;
        float moveAmount = 1f;
        Vector3 startPosition = DialoguePointer.transform.position;
        float threshold = 0.01f;

        while (DialoguePointer.activeSelf)
        {
            float step = moveSpeed * Time.deltaTime;
            Vector3 targetPosition = movingUp ? startPosition + new Vector3(0, moveAmount, 0) : startPosition;
            DialoguePointer.transform.position = Vector3.MoveTowards(DialoguePointer.transform.position, targetPosition, step);

            if (Vector3.Distance(DialoguePointer.transform.position, targetPosition) < threshold)
            {
                movingUp = !movingUp;
            }
            yield return null;
        }
    }
    
    public void HideTextbox()
    {
        textboxText.text = "";
        textboxPanel.SetActive(false);
        //SpeakerImage.SetActive(false);
        DialoguePointer.SetActive(false);
        StopCoroutine("AnimateDialoguePointer");
    }
}