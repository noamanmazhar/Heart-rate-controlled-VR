using System.Collections;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private void Start()
    {
        StartCoroutine(MoveCube());
    }

    IEnumerator MoveCube()
    {
        float t = 0.0f;
        Vector3 startingPosition = pointA.position;

        while (t < 1.0f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startingPosition, pointB.position, t);
            yield return null;
        }
    }
}