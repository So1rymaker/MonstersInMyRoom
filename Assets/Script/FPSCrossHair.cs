using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCrossHair : MonoBehaviour
{
    public float width;
    public float height;
    public float distance;
    public Texture2D crosshairTexture;

    private GUIStyle lineStyle;
    private Texture tex; 

    private void Start()
    {
        lineStyle = new GUIStyle();
        lineStyle.normal.background = crosshairTexture; 
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - distance / 2 - width, Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 + distance / 2, Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 - height / 2, Screen.height / 2 - distance / 2 - width, height, width), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 - height / 2, Screen.height / 2 + distance / 2, height, width), tex, lineStyle);
    }
}

