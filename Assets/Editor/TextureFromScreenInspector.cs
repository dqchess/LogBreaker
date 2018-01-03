using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TexteureFromScreen))]
public class TextureFromScreenInspector : Editor {

    TexteureFromScreen textureFromScreen;

    private void OnEnable()
    {
        textureFromScreen = target as TexteureFromScreen;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Take Picture"))
        {
            //Debug.Log("foto");
            ScreenCapture.CaptureScreenshot("Assets/backg.png");
        }
    }
}
