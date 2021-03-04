using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;


    private void Update()
    {
        transform.Rotate(0.0f, rotateSpeed, 0.0f);
    }
}
