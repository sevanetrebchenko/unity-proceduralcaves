using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MarchingCubes))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MarchingCubes marchingCubes = (MarchingCubes)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            Debug.Log("Generating mesh...");
            marchingCubes.GenerateMesh();
        }
    }
}
