using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavecontroller : MonoBehaviour
{

    public manager _manager;

    public string _received;

    public Material watermaterial;

    public GameObject waterplane;


    float transitionDuration = 2.5f;
    bool isTransitioning = false;
    float targetWaveSpeed;
    float targetNormalStrength;
    float targetwatery;


    public Transform pointA;
    public Transform pointB;

    // public float _BMPThreshold = 70f;

    // Start is called before the first frame update
    void Start()
    {
        watermaterial.SetFloat("_WaveSpeed", 0.1f);
        watermaterial.SetFloat("_NormalStrength", 0.459f);
    }

    // Update is called once per frame
    void Update()
    {
        _received = _manager.received_message;

        // Try to convert _received to a float
        if (float.TryParse(_received, out float convertedFloat))
        {
            // Successfully converted to float
            Debug.Log("Converted float value: " + convertedFloat);

            // Now you can use the convertedFloat variable as needed
            // For example, you might want to use it in some calculations or conditions
        }
        else
        {
            // Handle the case where the conversion failed (invalid string)
            Debug.LogError("Unable to convert to float: " + _received);
        }




        if (convertedFloat <= 70f && !isTransitioning)
        {
            targetWaveSpeed = 0.1f;
            targetNormalStrength = 0.459f;
            targetwatery = 0.0f;
            MoveWaterToA();
            StartCoroutine(TransitionValues());
        }
        else if (convertedFloat >= 70f && !isTransitioning)
        {
            targetWaveSpeed = 0.2f;
            targetNormalStrength = 2.0f;
            targetwatery = 0.5f;
            MoveWaterToB();
            StartCoroutine(TransitionValues());
        }



        // Coroutine for smooth transition
        IEnumerator TransitionValues()
        {
            float startWaveSpeed = watermaterial.GetFloat("_WaveSpeed");
            float startNormalStrength = watermaterial.GetFloat("_NormalStrength");

            float timer = 0f;
            isTransitioning = true;

            Vector3 startingPosition = pointA.position;

            while (timer < transitionDuration)
            {
                timer += Time.deltaTime;

                // Calculate the interpolation factor between 0 and 1 based on time
                float t = Mathf.Clamp01(timer / transitionDuration);

                // Interpolate between the two sets of values
                watermaterial.SetFloat("_WaveSpeed", Mathf.Lerp(startWaveSpeed, targetWaveSpeed, t));
                watermaterial.SetFloat("_NormalStrength", Mathf.Lerp(startNormalStrength, targetNormalStrength, t));

                // waterplane.transform.position = Vector3.Lerp(startingPosition, new Vector3(pointB.position.x, targetwatery, pointB.position.z), t);




                yield return null; // Wait for the next frame
            }

            // Ensure that the final values are set
            watermaterial.SetFloat("_WaveSpeed", targetWaveSpeed);
            watermaterial.SetFloat("_NormalStrength", targetNormalStrength);

            // Set the final position
            // waterplane.transform.position = new Vector3(pointB.position.x, targetwatery, pointB.position.z);

            isTransitioning = false; // Reset the transition flag
        }
    }

    // Move waterplane to point A
    void MoveWaterToA()
    {
        StartCoroutine(MoveWaterToPoint(pointA.position, transitionDuration));
    }

    void MoveWaterToB()
    {
        StartCoroutine(MoveWaterToPoint(pointB.position, transitionDuration));
    }

    // Coroutine to smoothly move waterplane to a target position with a slower speed
    // Coroutine to smoothly move waterplane to a target position with a similar time interval as TransitionValues
    IEnumerator MoveWaterToPoint(Vector3 targetPosition, float moveDuration)
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(waterplane.transform.position, targetPosition);

        while (waterplane.transform.position != targetPosition)
        {
            float elapsed_time = Time.time - startTime;

            // Calculate fracJourney using the provided moveDuration
            float fracJourney = Mathf.Clamp01(elapsed_time / moveDuration);

            // You can adjust the speed by multiplying fracJourney by a factor
            float slowedFracJourney = fracJourney * 0.005f; // Change 0.005f to your desired speed factor

            waterplane.transform.position = Vector3.Lerp(waterplane.transform.position, targetPosition, slowedFracJourney);

            yield return null;
        }
    }

}

    