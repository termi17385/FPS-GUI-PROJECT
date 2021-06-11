using System.Collections;
using System.Collections.Generic;

public static class Debugging
{
    public static bool debugMode = false;
    public static void EnableDebugMode() => debugMode = !debugMode;
    public static void DisableOnStart() => debugMode = false;
}
