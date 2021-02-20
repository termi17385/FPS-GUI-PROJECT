using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    public int currentHealth = 3;
    [SerializeField] Material mat;

    public void Damage(int DmgAmt)
    {
        currentHealth -= DmgAmt;
        mat.SetColor("_Color", Color.blue);
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
