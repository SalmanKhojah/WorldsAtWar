using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
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
    public TMP_Text segmentText;
    public float fadeDuration = 0.5f;
    public CinemachineVirtualCamera cameraOmarPlanet;
    private Animator _omarPlanetGFXAnimator;
    private bool isFading = false;


    
    public void Initialize()
    {
        

        _playerMainManager = FindObjectOfType<PlayerMainManger>();
        switchit = false;
        blackScreenImage = blackScreen.GetComponent<Image>();
        segmentText.gameObject.SetActive(false);
        _omarPlanetGFXAnimator = transform.GetChild(0).GetComponent<Animator>();



    }
    public void UpdateScript()
    {
        _inHatch = _playerMainManager.InHatch;

        if (_inHatch)
        {
            StartCoroutine(AnimateDialogueMessage());
            
        }

    }
    IEnumerator SwitchToOmarPlanetCamera(CinemachineVirtualCamera cameraOmarPlanet)
    {
        yield return new WaitForSeconds(2.0f);
        cameraOmarPlanet.Priority = 11; 
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeFromBlack());
        _omarPlanetGFXAnimator.Play("OmarPlanetExplode", 0, 0f);
        yield return new WaitForSeconds(1.51f);
        _omarPlanetGFXAnimator.Play("OmarPlanetExplodeIdle2", 0, 0f);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(3.0f);
        cameraOmarPlanet.Priority = 4; 
        yield return new WaitForSeconds(4.0f);
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

    pressXsoundsource.Stop();
    DialogueImage.transform.localScale = originalScale;
    DialogueImage.SetActive(false);

    textboxText.text = "";
    textboxText.color = originalTextColor; 
    yield return StartCoroutine(FadeToBlack());
    yield return SwitchToOmarPlanetCamera(cameraOmarPlanet);
    
    switchit = true;
    segmentText.gameObject.SetActive(true);
    yield return new WaitForSeconds(3f);
    


    segmentText.gameObject.SetActive(false);

    yield return new WaitForSeconds(2f);

    yield return StartCoroutine(FadeFromBlack());
    
    
}


private IEnumerator FadeToBlack()
{
    if (isFading) yield break; 
    isFading = true;
    blackScreen.SetActive(true);
    float elapsedTime = 0f;
    Color startColor = blackScreenImage.color;
    Color endColor = Color.black;
    while (elapsedTime < fadeDuration)
    {
        blackScreenImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    blackScreenImage.color = endColor;
    isFading = false;
}

private IEnumerator FadeFromBlack()
{
    if (isFading) yield break; 
    isFading = true;
    float elapsedTime = 0f;
    Color startColor = blackScreenImage.color;
    Color endColor = new Color(0, 0, 0, 0);
    while (elapsedTime < fadeDuration)
    {
        blackScreenImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    blackScreenImage.color = endColor;
    blackScreen.SetActive(false);
    isFading = false;
}


}