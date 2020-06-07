using UnityEngine;
using System.Collections.Generic;
public class CatmullRomSpline : MonoBehaviour
{
    [SerializeField] private CatmullRomControlPoint[] controlPoints;
    private List<Vector3> splinePoints;
    private bool isLooping;
    public static int n_Points = 5;

    private void Start()
    {
        Create();
    }
    private void OnDrawGizmosSelected()
    {
        foreach (CatmullRomControlPoint controlPoint in controlPoints) Gizmos.DrawSphere(controlPoint.transform.position, .2f);
        if (splinePoints != null)
        {
            for (int i = 0; i < splinePoints.Count - 1; i++)
            {
                Gizmos.DrawLine(splinePoints[i], splinePoints[i + 1]);
            }
        }
    }
    public void Create()
    {
        splinePoints = new List<Vector3>();
        isLooping = controlPoints[0].transform.position == controlPoints[controlPoints.Length - 1].transform.position;

        Vector3 endPointLeft;
        Vector3 midPointLeft;
        Vector3 midPointRight;
        Vector3 endPointRight;

        if(isLooping)
        {
            //connect the first one with the second one
            splinePoints.AddRange(GetSplineBetweenPoints(controlPoints[controlPoints.Length - 1].transform.position, controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position));
        }

        for (int i = 1; i <= controlPoints.Length-3; i++)
        {
            endPointLeft = controlPoints[i - 1].transform.position;
            midPointLeft = controlPoints[i].transform.position;
            midPointRight = controlPoints[i + 1].transform.position;
            endPointRight = controlPoints[i + 2].transform.position;
            splinePoints.AddRange(GetSplineBetweenPoints(endPointLeft, midPointLeft, midPointRight, endPointRight));
        }
        if(isLooping)
        {
            //connect the penultimate one with the last one
            splinePoints.AddRange(GetSplineBetweenPoints(controlPoints[controlPoints.Length - 3].transform.position, controlPoints[controlPoints.Length - 2].transform.position, controlPoints[controlPoints.Length - 1].transform.position, controlPoints[0].transform.position));
            //connect the last one with the first one
            splinePoints.AddRange(GetSplineBetweenPoints(controlPoints[controlPoints.Length - 2].transform.position, controlPoints[controlPoints.Length - 1].transform.position, controlPoints[0].transform.position, controlPoints[1].transform.position));
        }
    }
    public static Vector3[] GetSplineBetweenPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        Vector3[] points = new Vector3[n_Points];
        float resolution = 1f / (n_Points-1);
        for (int i = 0; i < n_Points; i++)
        {
            float x = i * resolution;
            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            points[i] = 0.5f * (a + (b * x) + (c * x * x) + (d * x * x * x));
        }

        return points;
    }
}
