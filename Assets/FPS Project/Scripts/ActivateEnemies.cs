using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
                enemy.GetComponent<Enemy>().target = other.transform;
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}