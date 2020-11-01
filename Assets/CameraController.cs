using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float distance = 8f;
    public static float angle = 40f;
    public static float interpolationSpeed = .5f;

    private static CameraController _instance;
    private static Coroutine currentCoroutine = null;
    private static UnityTemplateProjects.SimpleCameraController simpleCameraController;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        simpleCameraController = _instance.GetComponent<UnityTemplateProjects.SimpleCameraController>();
    }
    public static void LookAt(Transform target)
    {
        simpleCameraController.enabled = false;
        if (currentCoroutine != null) _instance.StopCoroutine(currentCoroutine);
        currentCoroutine = _instance.StartCoroutine(LookAtCoroutine(target, distance, angle, interpolationSpeed));
    }
    private static IEnumerator LookAtCoroutine(Transform target, float distance, float angle, float interpolationSpeed)
    {
        float distanceXZ = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float distanceY = distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 direction = (new Vector3(target.position.x, 0f, target.position.z) - _instance.transform.position).normalized;
        Vector3 endPosition = target.position + new Vector3(-direction.x * distanceXZ, 1f * distanceY, -direction.z * distanceXZ);
        
        while (_instance.transform.position != endPosition)
        {
            _instance.transform.position = Vector3.MoveTowards(_instance.transform.position, endPosition, interpolationSpeed);
            _instance.transform.LookAt(target);
            yield return null;
        }
        currentCoroutine = null;
        simpleCameraController.enabled = true;
    }
}
