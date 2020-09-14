using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float distance = 5f;
    public static float interpolationSpeed = 2f;

    private static CameraController _instance;
    private static Coroutine currentCoroutine = null;
    private void Awake()
    {
        if (_instance == null) _instance = this;
    }
    public static void LookAt(Vector3 position)
    {
        if (currentCoroutine != null) _instance.StopCoroutine(currentCoroutine);
            currentCoroutine = _instance.StartCoroutine(LookAtCoroutine(position, distance, interpolationSpeed));        
    }
    private static IEnumerator LookAtCoroutine(Vector3 position, float distance, float interpolationSpeed)
    {
        Vector3 direction = (position - _instance.transform.position).normalized;
        Vector3 endPosition = position - direction * distance;

        while (_instance.transform.position != endPosition)
        {
            _instance.transform.position = Vector3.MoveTowards(_instance.transform.position, endPosition, interpolationSpeed);
            yield return null;
        }
        currentCoroutine = null;
    }
}
