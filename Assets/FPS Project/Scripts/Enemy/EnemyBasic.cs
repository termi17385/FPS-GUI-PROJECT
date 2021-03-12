using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Utils;
using UnityEngine.UI;

public class EnemyBasic : MonoBehaviour
{
    #region Variables
    #region Ranges
    [SerializeField] protected float playerRangeMax;
    [SerializeField] protected float playerRangeMid;
    [SerializeField] protected float playerRangeMin;
    #endregion
    #region Floats
    [SerializeField] protected float playerDistance;
    protected float detectionLevel = 0;
    protected float maxDetectionLevel = 100;
    [SerializeField] protected float detectionAmt = 15;

    [SerializeField] private float x;
    public float angle;

    [SerializeField] float lineDist;
    [SerializeField] float lineOffsetDist;
    #endregion
    #region Transforms
    public Transform target;
    [SerializeField] protected Transform canvas;
    #endregion
    #region Bools
    public bool playerDetected = false;
    protected bool isPlayer;
    protected bool ray;
    #endregion
    #region Misc
    [SerializeField] protected Image bar;
    [SerializeField] protected LayerMask ignore;
    [SerializeField] protected NavMeshTargettingTest engageAi;
    #endregion
    #endregion

    #region Start and Update
    protected virtual void Start()
    {
        engageAi.enabled = false;
    }

    protected virtual void Update()
    {
        DetectPlayer();
        if (Debugging.debugMode){DebugDrawLines();}
        canvas.LookAt(target);
        //DetectionBar(detectionLevel);
    }
    #endregion

    #region Handles Detection
    private void DetectPlayer()
    {
        MathUtils.CalculateTargetAngle(transform, target, out angle, out playerDistance);
        RaycastHit _hit;

        // if the angle is less then x 
        if (angle < x && playerDistance < 50)
        {
            // send ray to the target to check if there is line of sight
            if (Physics.Linecast(transform.position, target.position, out _hit, ignore))
            {
                // if the enemy can see the player and 
                // no object is blocking view 
                // run method
            }
            Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
        }
       

        
        //Debug.Log("Target Angle" + angle + "\n Target Distance" + playerDistance);

        #region OldCode
        ////bool playerSpotted = Physics.Linecast(transform.position, target.position);
        //playerDistance = Vector3.Distance(transform.position, target.position); // gets the distance from point A to point B
        //RaycastHit hit;  // hit info
        //
        //if (playerDetected == true)   // checks if the player has entered the cone
        //{
        //    ray = Physics.Linecast(transform.position, target.position, out hit, ignore); // checks if there is line of sight to the player
        //    Debug.DrawRay(transform.position, target.position - transform.position, Color.red); // draws a debuging ray
        //    
        //    GameObject _target = hit.collider.gameObject; // gets the gameobject of what has been hit
        //    isPlayer = _target.CompareTag("Player"); // checks if that gameObject has tag player
        //
        //    PlayerDetectionSwitch(isPlayer); // calls method
        //    if (detectionLevel >= 100){canvas.gameObject.SetActive(true); engageAi.enabled = true;}  // checks if the player is max detection then sicks the enemy on them
        //}
        //
        //if (playerDetected == false)
        //{
        //    FillDetection(-detectionAmt * Time.deltaTime);
        //    canvas.gameObject.SetActive(false);
        //
        //    if (detectionLevel <= 0){ engageAi.enabled = false; } // stop chasing nothing you fool
        //} 
       
            //if (isPlayer)
            //{
            //Debug.Log(playerDetected);
            //Debug.Log(_target.name);

            //#region Player Distances

            //#endregion
            //}

            //if (isPlayer == false)
            //{
            //FillDetection(-detectionAmt * Time.deltaTime);
            //Debug.Log(_target.name);
            //canvas.gameObject.SetActive(false);
            //}
            #endregion
    }

    void DebugDrawLines()
    {
        float distance = lineDist;                              // used to make it go further out
        float offsetDistance = lineOffsetDist;                  // used to spread out the lines
        Vector3 offset = transform.right * offsetDistance;      // sets the offset
        Vector3 forward = transform.forward * distance;         // sets the direction

        Debug.DrawLine(transform.position, transform.position + forward - offset, Color.blue);   // left
        Debug.DrawLine(transform.position, transform.position + forward, Color.blue);            // middle
        Debug.DrawLine(transform.position, transform.position + forward + offset, Color.blue);   // right
    }

    #region Old
    /// <summary>
    /// Used to handle if the player is seen or not
    /// </summary>
    /// <param name="playerSpotted">A bool for checking if the player can be seen</param>
    private void PlayerDetectionSwitch(bool playerSpotted)
    {
        switch (playerSpotted)
        {
            case true:  // if true
            Debug.Log("I have line of sight on");
            PlayerDistances(); // run method
            break;

            case false:  // if false
            FillDetection(-detectionAmt * Time.deltaTime); // run method
            canvas.gameObject.SetActive(false);  // disable object

            if (detectionLevel <= 0){ engageAi.enabled = false; } // stop chasing nothing you fool
            break;
        }
    }

    /// <summary>
    /// Handles checking how far the target is from the enemy
    /// </summary>
    private void PlayerDistances()
    {
        #region Variables
        bool playerIsFar = playerDistance >= playerRangeMax;
        bool playerIsMidRange = playerDistance <= playerRangeMid && playerDistance >= playerRangeMin;
        bool playerIsClose = playerDistance <= playerRangeMin;
        #endregion

        if (playerIsFar)
        {
            FillDetection(detectionAmt * Time.deltaTime);
        }
        if (playerIsMidRange)
        {
            FillDetection(detectionAmt * Time.deltaTime * 2);
        }
        if (playerIsClose)
        {
            FillDetection(100);
        }
    }    
    private void DetectionBar(float spotLevel)
    {
        bar.fillAmount = Mathf.Clamp01(spotLevel / maxDetectionLevel);
        if (detectionLevel <= 0)
        {
            detectionLevel = 0;
        }
        if (detectionLevel >= 100)
        {
            detectionLevel = 100;
        }
    }
    private void FillDetection(float amt)
    {
        detectionLevel += amt;
    }
    #endregion
    #endregion
}
