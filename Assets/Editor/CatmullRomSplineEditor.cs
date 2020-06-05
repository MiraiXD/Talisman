using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CatmullRomSpline))]
public class CatmullRomSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create"))
        {
            (target as CatmullRomSpline).Create();
        }
    }
}
