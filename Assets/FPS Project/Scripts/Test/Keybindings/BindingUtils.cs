using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BindingUtils
{
    public static string TranslateKeyCode(KeyCode _code)
    {
        switch (_code)
        {
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Keypad0: return "Numpad 0";
            case KeyCode.Keypad1: return "Numpad 1";
            case KeyCode.Keypad2: return "Numpad 2";
            case KeyCode.Keypad3: return "Numpad 3";
            case KeyCode.Keypad4: return "Numpad 4";
            case KeyCode.Keypad5: return "Numpad 5";
            case KeyCode.Keypad6: return "Numpad 6";
            case KeyCode.Keypad7: return "Numpad 7";
            case KeyCode.Keypad8: return "Numpad 8";
            case KeyCode.Keypad9: return "Numpad 9";
            case KeyCode.LeftControl: return "Left Ctrl";
            case KeyCode.RightControl: return "Right Ctrl";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Return: return "Enter";
            case KeyCode.KeypadEnter: return "Numpad Enter";
            case KeyCode.UpArrow: return "Up";
            case KeyCode.RightArrow: return "Right";
            case KeyCode.DownArrow: return "Down";
            case KeyCode.LeftArrow: return "Left";
            case KeyCode.Mouse0: return "Left Click";
            case KeyCode.Mouse1: return "Right Click";
            case KeyCode.Mouse2: return "Middle Click";
            case KeyCode.LeftShift: return "Left Shift";
            case KeyCode.RightShift: return "Right Shift";
 
            default: return _code.ToString();
        }
    }

    public static KeyCode GetAnyPressedKey()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    public static void UpdateTextWithBinding(string _binding, TextMeshProUGUI _text)
    {
        _text.text = BindingManager.GetBinding(_binding).ValueDisplay;
    }
}
