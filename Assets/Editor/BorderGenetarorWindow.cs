using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BorderGenetarorWindow : EditorWindow {

    private BorderGenerator borderGenerator;

	public static EditorWindow ShowWindow()
    {
        return GetWindow(typeof(BorderGenetarorWindow));
    }

    private void OnEnable()
    {
        borderGenerator = GameManager.FindObjectOfType<BorderGenerator>();
       
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        borderGenerator.brickHeight = EditorGUILayout.FloatField("Brick Height", borderGenerator.brickHeight);
        borderGenerator.wallWidth = EditorGUILayout.IntField("Wall Width", borderGenerator.wallWidth);
        borderGenerator.wallHeight = EditorGUILayout.IntField("Wall Height", borderGenerator.wallHeight);
        if(GUILayout.Button("Generate Border"))
        {
            borderGenerator.GenerateWall();
        }

        if (GUILayout.Button("Apply Border"))
        {
            borderGenerator.ApplyWall();
        }


        GUILayout.EndVertical();
    }
}
