using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Start : MonoBehaviour
{
    bool u;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HoopTimer.b = false;
            u = true;

            while(u == true)
            {
                if ((HoopTimer.x < HoopTimer.y) || HoopTimer.y == 0)
                {
                    HoopTimer.y = HoopTimer.x;;
                    u = false;
                }
                break;
            }
        }
    }
}
