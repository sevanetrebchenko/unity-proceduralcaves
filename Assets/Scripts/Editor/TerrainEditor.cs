using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chunk))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        /*

            Chunk marchingCubes = (Chunk)target;

            if (GUILayout.Button("Generate Mesh"))
            {
                Debug.Log("Generating mesh...");
                marchingCubes.GenerateMesh();
            }
        }
        */

    }
}
