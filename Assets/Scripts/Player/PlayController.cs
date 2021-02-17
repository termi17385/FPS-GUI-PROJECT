using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    #region Variables
    public float speed;
    public float mouse_Speed;
    public float jumpHeight;

    public Transform cam;
    public Rigidbody rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        rb.velocity = transform.forward * v;

        float mouseX = Input.GetAxis("Mouse X") * mouse_Speed;
        float mouseY = Input.GetAxis("Mouse Y") * mouse_Speed;

        transform.Rotate(0, mouseX, 0);
        cam.transform.Rotate(-mouseY, 0, 0);
        
    }
}
