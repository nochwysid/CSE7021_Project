using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PopUpMessage
{
    public string message;
    [MenuItem("Custom Menu/Play")]
    public static void Play()
    {
        bool playOutput = EditorUtility.DisplayDialog("Play","keep playing", "yes","no");
        if (playOutput)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            EditorUtility.DisplayDialog("continue", "You WILL keep playing", "okay");
        }
    }
}
