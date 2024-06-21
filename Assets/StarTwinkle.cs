using System.Collections;
using UnityEngine;

public class StarTwinkle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float twinkleSpeed = 1f;
    private float maxBrightness = 1f;
    private float minBrightness = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Twinkle());
    }

    IEnumerator Twinkle()
    {
        while (true)
        {
            float targetBrightness = Random.Range(minBrightness, maxBrightness);
            float currentBrightness = spriteRenderer.color.a;
            while (Mathf.Abs(targetBrightness - currentBrightness) > 0.01f)
            {
                currentBrightness = Mathf.MoveTowards(currentBrightness, targetBrightness, twinkleSpeed * Time.deltaTime);
                spriteRenderer.color = new Color(1f, 1f, 1f, currentBrightness);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}