using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region CameraLook Variables
    [SerializeField] private Camera cam;
    [SerializeField] private float camSpeed_X;
    [SerializeField] private float camSpeed_Y;
    [SerializeField] private float mouseLock;
    [SerializeField] private Vector2 rotation = Vector2.zero;
    #endregion
    #region CameraMovement Variables
    [SerializeField] private float flightSpeed;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float boost;

    #endregion

    private void Start()
    {

    }

    private void Update()
    {
        FreeLook();
        FreeFly();
    }

    /// <summary>
    /// Handles the movement of the free cam 
    /// </summary>
    private void FreeFly()
    {
        // speed variables
        float moveSpeed;
        float upDownMoveSpeed;

        // handles increasing the speed of the object when shift is held
        if(Input.GetKey(KeyCode.LeftShift)){ moveSpeed = boost; upDownMoveSpeed = boost;}
        else{ moveSpeed = flightSpeed; upDownMoveSpeed = upDownSpeed;}

        // axis's being used to control the object
        float ws = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float ad = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        // how the object moves
        transform.Translate(Vector3.forward * ws);
        transform.Translate(Vector3.right * ad);

        // used to make the object go up and down
        if(Input.GetButton("Jump")){transform.Translate(Vector3.up * upDownMoveSpeed * Time.deltaTime);}
        if(Input.GetKey(KeyCode.LeftControl)){transform.Translate(Vector3.down * upDownMoveSpeed * Time.deltaTime);}

        #region Redacted
        //if (Input.GetKey(KeyCode.W))
        //{
        //transform.Translate(Vector3.forward * flightSpeed * Time.deltaTime);
        //}
        #endregion
    }

    /// <summary>
    /// handles the looking around of the free cam
    /// </summary>
    private void FreeLook()
    {
        rotation.x += Input.GetAxis("Mouse Y");
        rotation.y += Input.GetAxis("Mouse X");

        rotation.x = Mathf.Clamp(rotation.x, -mouseLock, mouseLock);
        transform.eulerAngles = new Vector2(-rotation.x, rotation.y) * camSpeed_Y;
       
        #region Redacted
        //transform.eulerAngles = new Vector2(rotation.x, 0) * camSpeed_X;
        //cam.transform.localRotation = Quaternion.Euler(rotation.x * -camSpeed_X, 0, 0);
        #endregion
    }
}
