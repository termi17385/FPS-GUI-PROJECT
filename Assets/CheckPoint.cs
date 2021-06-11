using System;
using System.Collections;
using System.Collections.Generic;
using FPSProject.Keybinds;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject pressEToSave;
    [SerializeField] private bool inRange;

    private void Update() => SavePlayerPosition();
    private void SavePlayerPosition()
    {
        if (BindingManager.BindingPressed("Interact") && inRange)
        {
            Debug.Log("SAVE");
        }
    }
    
    // handles checking if the player is in range of the checkpoint
    private void OnTriggerStay(Collider other) => pressEToSave.SetActive((inRange = other.CompareTag("Player")));
    private void OnTriggerExit(Collider other) => pressEToSave.SetActive((inRange = !other.CompareTag("Player")));
}
