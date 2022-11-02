using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPhotograph : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string path = Application.dataPath;
            ScreenCapture.CaptureScreenshot(path + "/StartSceneBackground.png");
        }
    }
}
