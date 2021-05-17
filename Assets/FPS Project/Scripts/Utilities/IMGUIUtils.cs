using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IMGUIUtils
{
    /// <summary>
    /// Used to set the matrix of IMGUI elements for scaling purposes needs a vector2
    /// </summary>
    /// <param name="_res">the screen resolution wanted</param>
    /// <returns>matrix 4x4</returns>
    public static Matrix4x4 IMGUIMatrix(Vector2 _res)
    {
        Vector2 screenResolution = new Vector2(_res.x, _res.y);
        var nativeSize = new Vector2(screenResolution.x, screenResolution.y);

        Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);
        //var matrix =
        return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
    }

    /// <summary>
    /// Used to set the matrix of IMGUI elements for scaling purposes needs to floats
    /// </summary>
    /// <param name="_res">the screen resolution wanted</param>
    /// <returns>matrix 4x4</returns>
    public static Matrix4x4 IMGUIMatrix(float _resX, float _resY)
    {
        Vector2 screenResolution = new Vector2(_resX, _resY);
        var nativeSize = new Vector2(screenResolution.x, screenResolution.y);

        Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);
        //var matrix =
        return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
    }
}
