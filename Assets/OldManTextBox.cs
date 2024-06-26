using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OldManTextBox : MonoBehaviour
{
    public GameObject textboxPanel;
    public GameObject black0;
    public GameObject black1;
    public GameObject black2;

    public GameObject black3;

    public GameObject black4;
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
            DialoguePointer.SetActive(false);
            StopCoroutine("AnimateDialoguePointer");
        }
        else if (!isTyping && textQueue.Count == 0)
        {
            DialoguePointer.SetActive(false);
            StopCoroutine("AnimateDialoguePointer");
        }
    }

    public void ShowTextbox(string text)
    {
        textboxPanel.SetActive(true);
        black0.SetActive(true);
        black1.SetActive(true);
        black2.SetActive(true);
        black3.SetActive(true);
        black4.SetActive(true);
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
        textboxText.text = "";
        float lastSoundTime = Time.time;

        foreach (char c in text)
        {
            textboxText.text += c;

            if (c != ' ' && typingSoundSource != null && typingSound != null && Time.time - lastSoundTime >= soundCooldown)
            {
                typingSoundSource.PlayOneShot(typingSound);
                lastSoundTime = Time.time;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        isTyping = false;

        if (textQueue.Count > 0)
        {
            StopCoroutine("AnimateDialoguePointer");
            DialoguePointer.SetActive(true);
            StartCoroutine("AnimateDialoguePointer");
        }
        if (textQueue.Count == 0)
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
        textboxPanel.SetActive(false);
        black0.SetActive(false);
        black1.SetActive(false);
        black2.SetActive(false);
        black3.SetActive(false);
        black4.SetActive(false);
        //SpeakerImage.SetActive(false);
        DialoguePointer.SetActive(false);
        StopCoroutine("AnimateDialoguePointer");
    }
}