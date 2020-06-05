using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour ,ISerializationCallbackReceiver
{
    [SerializeField] private CatmullRomSpline _outerMapSpline;
    private static CatmullRomSpline outerMapSpline;

    public static void CreateSplines()
    {
        outerMapSpline.Create();
    }

    public void OnBeforeSerialize()
    {        
    }

    public void OnAfterDeserialize()
    {
        outerMapSpline = _outerMapSpline;
    }
}
