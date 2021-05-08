using System.Collections.Generic;
using FPSProject.Keybinds;
using System.Collections;
using UnityEngine;
using System;

public class BindsLayoutManager : MonoBehaviour
{
    [SerializeField] private Transform[] leftOrRight;
    [SerializeField] private GameObject bindButton;

    private List<string> bindingNames = new List<string>();
    private List<string> buttonNames = new List<string>();

    [SerializeField] private int index = 0;
    [SerializeField] private bool even;
    [NonSerialized] private bool bindsEnabled = false;

    #region Debugging
    private Vector2 nativeSize;
    private float resWidth = 1280;
    private float resHeight = 720;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        foreach(Binding binding in BindingManager.instance.defaultBindings)
        {
            buttonNames.Add(binding.Name);
        }

        SpawnButton(bindButton);
    }

    private void SpawnButton(GameObject _button)
    {
        // spawn a set of buttons with  the corrosponding keybind settings
        // make sure they are spawned either left or right
        for(int i = 0; i < buttonNames.Count; i++)
        {
            Instantiate(_button, leftOrRight[i % 2]);
            BindingButton _bindName = _button.GetComponent<BindingButton>();
            _bindName.bindingToMap = buttonNames[i];
            #region Redunant Code
            //even = index % 2 == 0 ? true : false;
            //switch (even)
            //{
            //    case true:
            //    Instantiate(_button, 
            //    leftOrRight[0]);
            //    break;

            //    case false:
            //    Instantiate(_button, 
            //    leftOrRight[1]);
            //    break;
            //}
            //index = i;
            #endregion
        }
    }

    private void OnGUI()
    {
        // keeps everything scaled to the native size
        //nativeSize = new Vector2(resWidth, resHeight);                                          // used to set the native size of the image
        //Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);   // gets the scale of the screen
        //GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, scale);                   // sets the matrix and scales the GUI accordingly


        //if (GUI.Button(new Rect(100, 500, 100, 100), "PressMe"))
        //{
        //    if(!bindsEnabled) SpawnButton(bindButton);
        //    bindsEnabled = true;
        //}
    }
}
