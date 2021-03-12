using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectionBasic : MonoBehaviour
{
    EnemyBasic setDetection;

    private void Start()
    {
        setDetection = GetComponentInParent<EnemyBasic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (setDetection.target == null){setDetection.target = other.gameObject.transform;}
            //setDetection.playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //setDetection.playerDetected = false;
        }
    }
}
