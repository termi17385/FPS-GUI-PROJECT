using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging
{
    static public bool debugMode = false;

    private void Start()
    {
        debugMode = false;
    }

    static public void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugMode = !debugMode;
        } 
    }
}
