using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScenePhase : MonoBehaviour
{
    public Camera cameraToCapture;
    public RectTransform puzzleParent; // Parent UI element to hold puzzle pieces
    public float moveDistance = 100f; // Distance to move puzzle pieces
    public float animationTime = 5f; // Time for moving animations
    public float mergeTime = 3f; // Time for merge animation
    private GameObject background; // Reference to the background object
    private GameObject blinkScreen; // Reference to the blink screen object
    private List<GameObject> leftPieces = new List<GameObject>();
    private List<GameObject> rightPieces = new List<GameObject>();

    public void StartPhaseScene()
    {
        StartCoroutine(CaptureAndAnimateScreenshot());
    }

    private IEnumerator CaptureAndAnimateScreenshot()
    {
        yield return new WaitForEndOfFrame(); // Ensure the frame is rendered

        RectTransform puzzleParentRectTransform = puzzleParent.GetComponent<RectTransform>();
        int canvasWidth = (int)puzzleParentRectTransform.rect.width;
        int canvasHeight = (int)puzzleParentRectTransform.rect.height;

        // Create and display a black background
        background = new GameObject("Background");
        background.transform.SetParent(puzzleParent, false);
        Image backgroundImage = background.AddComponent<Image>();
        backgroundImage.color = Color.black;
        RectTransform bgRectTransform = background.GetComponent<RectTransform>();
        bgRectTransform.anchorMin = new Vector2(0, 0);
        bgRectTransform.anchorMax = new Vector2(1, 1);
        bgRectTransform.sizeDelta = new Vector2(0, 0); // Match parent size

        // Capture Screenshot
        RenderTexture renderTexture = new RenderTexture(canvasWidth, canvasHeight, 24);
        cameraToCapture.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(canvasWidth, canvasHeight, TextureFormat.RGB24, false);
        cameraToCapture.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, canvasWidth, canvasHeight), 0, 0);
        screenshot.Apply();
        cameraToCapture.targetTexture = null;
        RenderTexture.active = null;

        int cuts = 20;
        int pieceHeight = canvasHeight / cuts;

        for (int i = 0; i < cuts; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int pieceWidth = canvasWidth / 2;
                Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight);
                pieceTexture.SetPixels(screenshot.GetPixels(j * pieceWidth, canvasHeight - (i + 1) * pieceHeight, pieceWidth, pieceHeight));
                pieceTexture.Apply();

                GameObject pieceObject = new GameObject($"PuzzlePiece_{i}_{j}");
                pieceObject.transform.SetParent(puzzleParent, false);
                Image pieceImage = pieceObject.AddComponent<Image>();
                pieceImage.sprite = Sprite.Create(pieceTexture, new Rect(0.0f, 0.0f, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f));
                RectTransform pieceRectTransform = pieceObject.GetComponent<RectTransform>();
                pieceRectTransform.sizeDelta = new Vector2(pieceWidth, pieceHeight);
                pieceRectTransform.anchoredPosition = new Vector2(j * pieceWidth - (pieceWidth / 2) + (canvasWidth % 2 == 0 ? 0 : 0.5f), -i * pieceHeight + (canvasHeight / 2 - pieceHeight / 2));

                if (j == 0) // Left piece
                {
                    leftPieces.Add(pieceObject);
                }
                else // Right piece
                {
                    rightPieces.Add(pieceObject);
                }
            }
        }

    
        yield return StartCoroutine(BlinkScreenFadeOut(animationTime));

        StartCoroutine(MergeAndSeparatePieces());
    }

    private IEnumerator BlinkScreenFadeOut(float duration)
    {
        blinkScreen = new GameObject("BlinkScreen");
        blinkScreen.transform.SetParent(puzzleParent, false);
        Image blinkScreenImage = blinkScreen.AddComponent<Image>();
        RectTransform blinkScreenRectTransform = blinkScreen.GetComponent<RectTransform>();
        blinkScreenRectTransform.anchorMin = new Vector2(0, 0);
        blinkScreenRectTransform.anchorMax = new Vector2(1, 1);
        blinkScreenRectTransform.sizeDelta = new Vector2(0, 0); // Match parent size
        float halfDuration = duration / 2;
        float timer = 0;

        // Fade to black
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / halfDuration);
            blinkScreenImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Fade to transparent
        timer = 0; // Reset timer for the fade out
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / halfDuration);
            blinkScreenImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator AnimatePieces(List<GameObject> pieces, Vector2 direction, float distance, float duration)
    {
        float startTime = Time.time;
        Vector2[] startPositions = new Vector2[pieces.Count];
        for (int i = 0; i < pieces.Count; i++)
        {
            startPositions[i] = pieces[i].GetComponent<RectTransform>().anchoredPosition;
        }

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            for (int i = 0; i < pieces.Count; i++)
            {
                Vector2 startPosition = startPositions[i];
                Vector2 horizontalDirection = new Vector2(direction.x, 0);
                Vector2 endPosition = startPosition + horizontalDirection * distance;
                float newXPosition = Mathf.Lerp(startPosition.x, endPosition.x, t);
                pieces[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(newXPosition, startPosition.y);
            }
            yield return null;
        }

        for (int i = 0; i < pieces.Count; i++)
        {
            Vector2 startPosition = startPositions[i];
            Vector2 horizontalDirection = new Vector2(direction.x, 0);
            Vector2 endPosition = startPosition + horizontalDirection * distance;
            pieces[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(endPosition.x, startPosition.y);
        }
    }

private IEnumerator MergeAndSeparatePieces()
{

    // Calculate merge direction (opposite to initial directions)
    Vector2 mergeDirectionFirst = new Vector2(1, 0);
    Vector2 mergeDirectionSecond = new Vector2(-1, 0);

    // Calculate distances for the initial merge
    float mergeDistance = 60; // Move a bit past merging

    // Initial move towards each other a bit past the merge point
    StartCoroutine(AnimatePieces(leftPieces, mergeDirectionFirst, mergeDistance, mergeTime));
    StartCoroutine(AnimatePieces(rightPieces, mergeDirectionSecond, mergeDistance, mergeTime));

    yield return new WaitForSeconds(mergeTime);

    // Corrected separation direction after initial merge
    Vector2 separationDirectionFirst = new Vector2(-1, 0); // Left
    Vector2 separationDirectionSecond = new Vector2(1, 0); // Right

    // Corrected separation distance and duration
    float separationDistance = 20; // Distance to move back after "merging"
    float separationDuration = 0.3f; // Corrected duration for the first separation

    // Corrected first separation
    StartCoroutine(AnimatePieces(leftPieces, separationDirectionFirst, separationDistance, separationDuration));
    StartCoroutine(AnimatePieces(rightPieces, separationDirectionSecond, separationDistance, separationDuration));

    yield return new WaitForSeconds(separationDuration);

    // Pause before final separation
    yield return new WaitForSeconds(0.05f);

    // Final separation and exit from canvas
    float finalSeparationDistance = puzzleParent.rect.width; // Assuming this is enough to exit the canvas
    float finalSeparationDuration = 0.1f; // Corrected duration for the final separation

    // Continue moving in the corrected separation direction until they exit the canvas
    StartCoroutine(AnimatePieces(leftPieces, separationDirectionFirst, finalSeparationDistance, finalSeparationDuration));
    StartCoroutine(AnimatePieces(rightPieces, separationDirectionSecond, finalSeparationDistance, finalSeparationDuration));
}
}