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
    public float fadeDuration = 0f;
    public CinemachineVirtualCamera cameraOmarPlanet;
    private Animator _omarPlanetGFXAnimator;


    
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
        cameraOmarPlanet.Priority = 11; 
        yield return new WaitForSeconds(2);
        _omarPlanetGFXAnimator.Play("OmarPlanetExplode", 0, 0f);
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(1);
        _omarPlanetGFXAnimator.Play("OmarPlanetIdle", 0, 0f);
        cameraOmarPlanet.Priority = 4; 
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


    yield return SwitchToOmarPlanetCamera(cameraOmarPlanet);
    

    segmentText.gameObject.SetActive(true);
    yield return new WaitForSeconds(3f);
    
    DialogueImage.transform.localScale = originalScale;
    DialogueImage.SetActive(false);

    textboxText.text = "";
    textboxText.color = originalTextColor; 
    pressXsoundsource.Stop();
    switchit = true;

    segmentText.gameObject.SetActive(false);

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