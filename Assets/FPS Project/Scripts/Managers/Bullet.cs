using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private int hitCount = 0;
    [SerializeField] private float hitForce = 1000;
    [SerializeField] private float rayLength;

    private Transform bullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bullet = this.gameObject.transform;
    }

    void PushObject(Rigidbody _rb)
    {
        _rb.AddForce(rb.velocity * hitForce);
    }

    private void Update()
    {
        //rb.velocity = transform.forward * speed * Time.deltaTime;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)    // ground layer
        {
            Destroy(this.gameObject);
        }

        if (other.gameObject.layer == 7)    // object layer
        {
            PushObject(other.gameObject.GetComponent<Rigidbody>());
            Destroy(this.gameObject);
        }

        if (other.gameObject.layer == 8)    // Character layer
        {
            Destroy(this.gameObject);
        }
    }
}
