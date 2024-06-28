using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PressXAnimation : MonoBehaviour
{
    public AudioSource pressXsoundsource;
    public AudioClip xsound;
    public GameObject DialogueImage;
    private PlayerMainManger _playerMainManager; 
    private bool _inHatch;
    public TMP_Text textboxText;
    public bool switchit;

    public GameObject blackScreen;
    Image blackScreenImage;
    public float fadeDuration = 0.5f;


    
    public void Initialize()
    {
        

        _playerMainManager = FindObjectOfType<PlayerMainManger>();
        switchit = false;
        blackScreenImage = blackScreen.GetComponent<Image>();




    }
    public void UpdateScript()
    {
        _inHatch = _playerMainManager.InHatch;

        if (_inHatch)
        {
            StartCoroutine(AnimateDialogueMessage());
            
        }

    }
IEnumerator AnimateDialogueMessage()
{
    
    DialogueImage.SetActive(true);
    textboxText.text = "Attempt to open hatch";


    Vector3 originalScale = DialogueImage.transform.localScale;
    Vector3 minScale = originalScale * 0.95f;
    Vector3 maxScale = originalScale * 1.05f;
    float duration = 1.0f;
    float timer = 0.0f;

    Color originalTextColor = textboxText.color;
    Color targetTextColor = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0);

    while (_inHatch && DialogueImage.activeSelf)
    {
        while (timer <= duration)
        {
            float t = Mathf.PingPong(timer, duration / 2) / (duration / 2);
            DialogueImage.transform.localScale = Vector3.Lerp(minScale, maxScale, t);
            textboxText.color = Color.Lerp(originalTextColor, targetTextColor, t); 
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0.0f;

        if (pressXsoundsource != null && xsound != null)
        {
            pressXsoundsource.PlayOneShot(xsound);
        }
    }

    yield return StartCoroutine(FadeToBlack());
    
    DialogueImage.transform.localScale = originalScale;
    DialogueImage.SetActive(false);

    textboxText.text = "";
    textboxText.color = originalTextColor; 
    pressXsoundsource.Stop();
    switchit = true;

    yield return StartCoroutine(FadeFromBlack());
    
    
}

    private IEnumerator FadeToBlack()
    {
        blackScreen.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            blackScreenImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreenImage.color = Color.black;
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            blackScreenImage.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreenImage.color = new Color(0, 0, 0, 0);
        blackScreen.SetActive(false);
    }


}