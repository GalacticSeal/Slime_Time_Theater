using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    private static int gameFPS = 60;

    void Start()
    {
        //Unity API: https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = gameFPS; //sets target framerate to designated number
    }
}
