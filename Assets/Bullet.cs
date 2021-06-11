using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform targetLocation;
    public int damageAmount;

    private void Update() => TargetHit();
    private void TargetHit()
    {
        if(targetLocation != null)
        {
            Debug.LogWarning("TargetFound");
            
            float targetDistance = Vector3.Distance(transform.position, targetLocation.position);
            if (targetDistance <= 1)
            {
                if (targetLocation.GetComponent<Enemy>() == true)
                {
                    if(targetLocation.gameObject.activeSelf == true) 
                        targetLocation.GetComponent<Enemy>().Damage(damageAmount);
                }
                else targetLocation.GetComponent<target>().Damage(damageAmount);
                Destroy(gameObject);
            }
        }
        else
        {
            //StartCoroutine(Destroy());
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
}
