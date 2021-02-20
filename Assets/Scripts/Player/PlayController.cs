using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    #region Variables
    public float speed;

    public float jumpHeight;

    #region MouseStuff
    [Min(0)]
    [SerializeField] private float mouse_Speed;
    [SerializeField] Vector2 rotation = Vector2.zero;
    #endregion

    public Transform cam;
    public Rigidbody rb;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

       
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        MouseMovement();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        rb.velocity = transform.forward * v;
    }


    /// <summary>
    /// Handles looking around
    /// </summary>
    private void MouseMovement()
    {
        rotation.y += Input.GetAxis("Mouse X");     // sets rotation to work with mouse movement
        rotation.x += -Input.GetAxis("Mouse Y");    // sets rotation to work with mouse movement

        rotation.x = Mathf.Clamp(rotation.x, -15, 15);                                      // clamps the camera so you cant look up to far
        transform.eulerAngles = new Vector2(0, rotation.y) * mouse_Speed;                   // handles the rotation of the character times'd by the mouse speed
        cam.transform.localRotation = Quaternion.Euler(rotation.x * mouse_Speed, 0, 0);     // handles the local rotation of the camera times'd by mouse speed
    }
}
