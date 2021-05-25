using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EventTesting : SerializedMonoBehaviour
{
    public delegate void Triggered(float number);
    public static event Triggered onTriggered;

    public float number = 0;

    private bool _triggerEvent;

    [SerializeField]
    public bool triggerEvent
    {
        get
        {
            return _triggerEvent;
        }
        set
        {
            if(value == true)
            {
                onTriggered.Invoke(number);
            }

            _triggerEvent = value;
        }
    }

    private void Update()
    {
        number += 0.5f * Time.deltaTime;
    }
}           
