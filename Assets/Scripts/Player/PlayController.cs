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

    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    #endregion

    public Transform cam;

    [SerializeField] private CharacterController cc;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
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
        #region The axis being used to move the player
        float h = Input.GetAxis("Horizontal");      // left and right axis (a and d)
        float v = Input.GetAxis("Vertical");        // up and down axis ( w and s)
        #endregion

        Vector3 direction = new Vector3(h, 0 ,v).normalized;

        // if the length of the players move direction is more then 0.1
        if (direction.magnitude >= 0.1f)
        {
            #region Handles the rotation and movement of the player based on the camera's angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z)
            * Mathf.Rad2Deg + cam.eulerAngles.y;                                // gets the direction and camera facing angle                 

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 
            targetAngle, ref turnSmoothVelocity, turnSmoothTime);               // keeps the player smoothly looking in the direction of the camera  
            #endregion

            #region moves the player to face the direction of the camera so they always move camera facing
            transform.rotation = Quaternion.Euler(0f, angle, 0f);                             // rotates the player around the y axis
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;        // rotates the player to always be facing forward and keeps the movement going forward
            #endregion

            cc.Move(moveDir * speed * Time.deltaTime); // moves the player times'd be speed and Time.deltaTime
        }
        else
        {
            Debug.Log("stop");  // used to see when the player actually stops        
        }
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
