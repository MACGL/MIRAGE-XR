using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class LoadStartScene 
{
    [MenuItem("Tools/LoadStartScene %t")]
    public static void Do()
    {
        EditorSceneManager.OpenScene("Assets/MirageXR/Common/Scenes/Start.unity");
    }
}
