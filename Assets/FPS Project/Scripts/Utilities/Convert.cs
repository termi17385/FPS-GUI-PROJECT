using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Convert
{
    public static Vector3 FloatToVector3(float[] _floats)
    {
        return new Vector3(_floats[0], _floats[1], _floats[2]);
    }

    public static Vector2 FloatToVector2(float[] _floats)
    {
        return new Vector2(_floats[0], _floats[1]);
    }
}