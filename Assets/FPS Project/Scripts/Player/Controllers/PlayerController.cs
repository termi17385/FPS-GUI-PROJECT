using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float walkSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    public float jumpHeight;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Min(1)] private float gravMultiplier;

    #region MouseStuff
    [Min(0)]
    [SerializeField] private float mouse_Speed;
    [SerializeField] Vector2 rotation = Vector2.zero;
    [SerializeField] private float mouseLock;

    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    #endregion

    public Transform cam;

    [SerializeField] private CharacterController cc;
    [SerializeField] private PauseMenuHandler pause;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool grounded = false;

    [SerializeField] private GameManager gm;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.freeCamMode) // if in free cam mode stop the player moving while they use the camera
        {
            PlayerMovement();
            PlayerJumping();
            if (pause.paused == false)
            {
                MouseMovement();
            }
        }
    }

    private void PlayerMovement()
    {
        #region The axis being used to move the player
        float h = Input.GetAxisRaw("Horizontal");      // left and right axis (a and d)
        float v = Input.GetAxisRaw("Vertical");        // up and down axis ( w and s)
        #endregion

        Vector3 direction = new Vector3(h, 0 ,v).normalized;

        // if the length of the players move direction is more then 0.1
        if (direction.magnitude >= 0.1f)
        {
            #region Handles the rotation and movement of the player based on the camera's angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z)
            * Mathf.Rad2Deg + cam.eulerAngles.y;                                // gets the direction and camera facing angle                 

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 
            targetAngle, ref turnSmoothVelocity, turnSmoothTime);               // rotates the player slowly to the direction of the camera facing 
            #endregion

            #region moves the player to face the direction of the camera so they always move camera facing
            transform.rotation = Quaternion.Euler(0f, angle, 0f);                             // rotates the player around the y axis
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;        // rotates the player to always be facing forward and keeps the movement going forward
            #endregion

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            cc.Move(moveDir * moveSpeed * Time.deltaTime); // moves the player times'd be speed and Time.deltaTime
        }
        else
        {
            Debug.Log("stop");  // used to see when the player actually stops        
        }
    }

    /// <summary>
    /// Sets and handles the gravity of the player <br/>
    /// as well as handles if the player can jump or not
    /// </summary>
    private void PlayerJumping()
    {
        // checks if the player is grounded and stopped moving
        if (grounded && playerVelocity.y <= 0)
        {
            // if true reset the players velocity to 0;
            playerVelocity.y = 0f; 
        }

        // checks if the player pressed the jump button (space) and if they are ground
        if (Input.GetButtonDown("Jump") && grounded)
        {
            // if true the players velocity adds the sqrt of the jumpheight times'd by -3.0f and gravity
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        float gravity_ = gravity * gravMultiplier;

        playerVelocity.y += gravity_ * Time.deltaTime;      // this is the gravity of the player used to make the player fall down 
        cc.Move(playerVelocity * Time.deltaTime);           // moves the character controller up and down which moves the player 
    }


    /// <summary>
    /// This method is used to handle mouse look <br/>
    /// It does this by getting the mouse axis's times'd by speed and <br/>
    /// rotates the player depending on the value given from the mouse axis 
    /// <br/> <br/>
    /// THe method also clamps the x axis so the player doesnt look up too far
    /// </summary>
    private void MouseMovement()
    {
        rotation.y += Input.GetAxis("Mouse X");     // Assigns the Y axis to mouse X getting data from mouse movement
        rotation.x += -Input.GetAxis("Mouse Y");    // Assigns the X axis to mouse Y getting data from mouse movement

        rotation.x = Mathf.Clamp(rotation.x, -mouseLock, mouseLock);                        // clamps the camera so you cant look up too far
        transform.eulerAngles = new Vector2(0, rotation.y) * mouse_Speed;                   // Rotates the player around the y axis times'd by the mouse speed
        cam.transform.localRotation = Quaternion.Euler(rotation.x * mouse_Speed, 0, 0);     // Rotates the camera up and down the x axis times'd by mouse speed
    }

    #region CollisonStuff
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")  
        {
            grounded = false;
        }
    }
    #endregion
}