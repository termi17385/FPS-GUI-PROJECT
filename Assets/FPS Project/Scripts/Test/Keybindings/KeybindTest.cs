using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindTest : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = speed * Time.deltaTime;

        if (BindingManager.BindingHeld("Forward"))
        {
            transform.position += transform.up * moveSpeed;
        }
        if (BindingManager.BindingHeld("Left"))
        {
            transform.position += transform.right * -moveSpeed;
        }
        if (BindingManager.BindingHeld("Right"))
        {
            transform.position += transform.right * moveSpeed;
        }
        if (BindingManager.BindingHeld("Down"))
        {
            transform.position += transform.up * -moveSpeed;
        }
    }
}
