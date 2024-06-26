using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressXAnimation : MonoBehaviour
{
    public AudioSource pressXsoundsource;
    public AudioClip xsound;
    public GameObject pressX;
    public GameObject black0;
    public GameObject black1;
    public GameObject black2;

    public GameObject black3;

    public GameObject black4;
    private PlayerMainManger _playerMainManager; 
    private bool _inHatch;
    public TMP_Text textboxText;
    public bool switchit;

    public void Initialize()
    {
        

        _playerMainManager = FindObjectOfType<PlayerMainManger>();
        switchit =false;



    }
    public void UpdateScript()
    {
        _inHatch = _playerMainManager.InHatch;

        if (_inHatch)
        {
            StartCoroutine(AnimateDialoguePointer());
        }

    }
IEnumerator AnimateDialoguePointer()
{
    
    pressX.SetActive(true);
    GameObject[] blacks = {black0, black1, black2, black3, black4};
    foreach (var black in blacks)
    {
        black.SetActive(true);
    }

    Vector3 originalScale = pressX.transform.localScale;
    Vector3 minScale = originalScale * 0.95f;
    Vector3 maxScale = originalScale * 1.05f;
    float duration = 1.0f;
    float timer = 0.0f;

    Color originalTextColor = textboxText.color;
    Color targetTextColor = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0);

    while (_inHatch && pressX.activeSelf)
    {
        while (timer <= duration)
        {
            float t = Mathf.PingPong(timer, duration / 2) / (duration / 2);
            pressX.transform.localScale = Vector3.Lerp(minScale, maxScale, t);
            foreach (var black in blacks)
            {
                black.transform.localScale = Vector3.Lerp(minScale, maxScale, t); 
            }
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

    
    pressX.transform.localScale = originalScale;
    pressX.SetActive(false);
    foreach (var black in blacks)
    {
        black.transform.localScale = originalScale; 
        black.SetActive(false); 
        switchit = true;

    }
    textboxText.color = originalTextColor; 
}

}