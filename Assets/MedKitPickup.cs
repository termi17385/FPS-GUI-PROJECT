using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Stats;

public class MedKitPickup : MonoBehaviour
{
    private PlayerStats stats;
    [SerializeField] private bool canCollect = false;
    [SerializeField] private GameObject pressButtonText;
    [SerializeField] private Transform player;

    private void Start()
    {
        pressButtonText.SetActive(false);
        canCollect = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canCollect == true)
        {
            if (stats != null)
            {
                stats.medkits++;
                Destroy(this.gameObject);
            }
        }    

        if (canCollect == true)
        {
            pressButtonText.transform.LookAt(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stats = other.GetComponent<PlayerStats>();
            player = other.gameObject.transform;

            pressButtonText.SetActive(true);
            canCollect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pressButtonText.SetActive(false);
            canCollect = false;
        }
    }
}
