using System.Collections.Generic;
using FPSProject.Player.Manager;
using FPSProject.NPC.Dialogue;
using System.Collections;
using FPSProject.Keybinds;
using FPSProject.Menu;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FPSProject.Player
{
    public class PlayerController : SerializedMonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Handles the controls for left and right movement
        /// </summary>
        int GetAxisLeftRight
        {
            get
            {
                int getAxisRight = BindingManager.BindingHeld("Right") ? 1 : 0;
                int getAxisLeft = BindingManager.BindingHeld("Left") ? -1 : 0;
                return getAxisRight + getAxisLeft;
            }
        }
        /// <summary>
        /// Handles the controls for forward and backward
        /// </summary>
        int GetAxisForwardBack
        {
            get
            {
                int getAxisForward = BindingManager.BindingHeld("Forward") ? 1 : 0;
                int getAxisBackward = BindingManager.BindingHeld("Backward") ? -1 : 0;
                return getAxisForward + getAxisBackward;
            }
        }
        #endregion
        
        #region Variables
        [SerializeField] private GameObject inventoryMenu;
        [SerializeField] private PlayerManager pManager;
        [ReadOnly] public float moveSpeed;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField, Min(1)] private float gravMultiplier;

        [HideInInspector]public float sprintSpeed;
        [HideInInspector]public float crouchSpeed;
        [HideInInspector]public float walkSpeed;
        [SerializeField]public float jumpHeight;

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
        [SerializeField] private Vector3 playerVelocity;
        [SerializeField] private bool grounded = false;
        [SerializeField] private bool isTalking = false;
        [SerializeField] private bool inventoryOpened = false;

        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private Transform[] handPositions;
        [SerializeField] private Transform[] hands;
        #endregion

        #region  Start and Update
        // Start is called before the first frame update
        void Start()
        {
            cc = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Debugging.DisableOnStart();
            crouchSpeed = 2;
        }
        // Update is called once per frame
        void Update()
        {
            if (!PauseMenu.instance.death)
            {
                bool enableOrDisable = Input.GetKeyDown(KeyCode.F2);
                if (enableOrDisable) Debugging.EnableDebugMode();

                if (inventoryMenu.activeSelf == true)
                {
                    PauseMenu.instance.paused = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                PausedGame();
                TalkToNPC();
                OpenInventory();

                if (PauseMenu.instance.paused == false)
                {
                    grounded = GroundCheck();
                    PlayerMovement();
                    PlayerJumping();
                    MoveHandsIntoPosition();

                    if (!Debugging.debugMode)
                    {
                        if (Cursor.lockState == CursorLockMode.None)
                            Cursor.lockState = CursorLockMode.Locked;
                        MouseMovement();
                    }
                    else if (Debugging.debugMode)
                    {
                        if (Cursor.lockState == CursorLockMode.Locked)
                            Cursor.lockState = CursorLockMode.None;
                    }
                }
            }
        }
        #endregion
       
        #region quest and inventory
        private void OpenInventory()
        {
            if(BindingManager.BindingPressed("Inventory"))
            {
                inventoryOpened = !inventoryOpened;
                if (inventoryOpened)
                {
                    PauseMenu.instance.paused = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (!inventoryOpened)
                {
                    PauseMenu.instance.paused = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    pManager.saved = true;
                }
                inventoryMenu.GetComponent<InventoryMenuManager>().OnClick(0);
                inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            }
        }
        private void TalkToNPC()
        {
            if (BindingManager.BindingPressed("Interact"))
            {
                isTalking = !isTalking;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
                {
                    print(hit.transform.name);
                    if ((hit.transform.tag == "NPC" && isTalking))
                    {
                        Dialogue npcDialogue = hit.transform.GetComponent<Dialogue>();
                        if (npcDialogue)
                        {                                                 
                            DialogueManager.dM.LoadDialogue(npcDialogue);
                            PauseMenu.instance.paused = true;
                            Cursor.lockState = CursorLockMode.None;
                        }
                    }
                    //else if ((hit.transform.tag == "NPC" && isTalking))
                    //{
                    //    Dialogue npcDialogue = hit.transform.GetComponent<Dialogue>();
                    //    if (npcDialogue)
                    //    {
                    //        DialogueManager.dM.;
                    //    }
                    //}
                }
            }

            Debug.DrawRay(transform.position, (transform.forward * 3f), Color.red);
        }
        #endregion

        #region Misc and other Controls
        private void MoveHandsIntoPosition()
        {
            hands[0].transform.position = handPositions[0].transform.position;
            hands[1].transform.position = handPositions[1].transform.position;
        }

        /// <summary>
        /// Pauses the game when escape is pressed
        /// </summary>
        private void PausedGame()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
             PauseMenu.instance.PauseGame();
        }
        #endregion
        #region Movement
        // ReSharper disable Unity.PerformanceAnalysis
        private void PlayerMovement()
        {
            Vector3 direction = new Vector3(GetAxisLeftRight, 
                0, GetAxisForwardBack).normalized;
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

                moveSpeed = BindingManager.BindingHeld("Sprint") ? sprintSpeed : walkSpeed;
                moveSpeed = BindingManager.BindingHeld("Crouch") ? crouchSpeed : moveSpeed;
                cc.Move(moveDir * (moveSpeed * Time.deltaTime)); // moves the player times'd be speed and Time.deltaTime
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
            if (BindingManager.BindingPressed("Jump") && grounded)
            {
                // if true the players velocity adds the sqrt of the jumpheight times'd by -3.0f and gravity
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                grounded = false;
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
        #endregion
        #region CollisonStuff

        private bool GroundCheck()
        {
            RaycastHit hit;
            Vector3 pos = transform.position + cc.center;
                           
            //Physics.SphereCast(pos, 0.5f, Vector3.up, out hit, 0.5f, groundLayer))
            if (Physics.SphereCast(pos, cc.height / 2, -transform.up, out hit, 0.1f))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    return true;
                }
            }
            return false;
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.tag == "Ground")
        //    {
        //        grounded = true;
        //    }
        //}
        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.tag == "Ground")  
        //    {
        //        grounded = false;
        //    }
        //}
        #endregion
    }
}

