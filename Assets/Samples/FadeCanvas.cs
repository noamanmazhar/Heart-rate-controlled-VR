using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public RawImage rawImage;
    public float fadeDuration = 2.0f; // Adjust the duration as needed
    public float startDelay = 1.0f; // Adjust the delay before fading starts

    void Start()
    {
        // Start the delayed fade-in process
        StartCoroutine(StartFadeAfterDelay());
    }

    IEnumerator StartFadeAfterDelay()
    {
        // Wait for the specified delay before starting the fade
        yield return new WaitForSeconds(startDelay);

        // Start the fade-in process
        StartCoroutine(FadeRawImageCoroutine());
    }

    IEnumerator FadeRawImageCoroutine()
    {
        Color startColor = rawImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // Fully transparent

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Calculate the interpolation factor between 0 and 1 based on time
            float t = Mathf.Clamp01(timer / fadeDuration);

            // Interpolate between the start and target colors
            rawImage.color = Color.Lerp(startColor, targetColor, t);

            yield return null; // Wait for the next frame
        }

        // Ensure that the final color is set
        rawImage.color = targetColor;
    }
}
