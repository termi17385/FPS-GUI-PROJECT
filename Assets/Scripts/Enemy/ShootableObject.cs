using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    public int currentHealth = 3;

    public void Damage(int DmgAmt)
    {
        currentHealth -= DmgAmt;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
