using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class FlashFarm : MonoBehaviour
{
    public Image whiteScreen;
    public GameObject[] selectedGameObjects;
    public float flashDuration = 3f;
    private bool isFlashing = false;
    public bool switchToPostFlash;

    public void Initialize()
    {
        
        switchToPostFlash = false;
    }

    public void TriggerWhiteFlash()
    {

            StartCoroutine(FlashSequence());

    }

IEnumerator FlashSequence()
{
    whiteScreen.color = new Color(1, 1, 1, 0);
    isFlashing = true;
    // Make selected GameObjects black and visible
    foreach (var obj in selectedGameObjects)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.black;
        }
        else
        {
            var image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.black;
            }
        }
        obj.SetActive(true);
    }

    // Fade to white, using half the flashDuration for the fade in
    yield return StartCoroutine(FadeToColor(whiteScreen, Color.white, flashDuration / 2));
    // Fade back to transparent, using the other half of the flashDuration for the fade out
    yield return new WaitForSeconds(5f);
    switchToPostFlash = true;
    yield return StartCoroutine(FadeToColor(whiteScreen, new Color(1, 1, 1, 0), flashDuration / 2));


    isFlashing = false;
}

    IEnumerator FadeToColor(Image image, Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = targetColor;
    }
}