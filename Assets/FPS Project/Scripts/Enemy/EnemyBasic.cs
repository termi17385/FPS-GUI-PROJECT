using NaughtyAttributes;
using UnityEngine;  
using UnityEngine.AI;
using FPSProject.Utils;
using FPSProject.Player.Manager;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBasic : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// read only property for Angle
    /// </summary>
    protected float TargetAngle
    { 
        get => angle;
    }
    /// <summary>
    /// read only property for Max Angle
    /// </summary>
    protected float MaxAngle
    {
        get{return maxAngle = lineOffsetDist;}
        set{lineOffsetDist = value;}
    }
    /// <summary>
    /// Read only property for target distance
    /// </summary>
    protected float TargetDistance
    {
        get => targetDistance;
    }
    #endregion
    #region Variables
    #region angle and distance
    [Header("Angles and Distances"), 
    InfoBox("Read only variables for showing the current angle and current distance to target")]
    [SerializeField, ReadOnly] private float angle;
    [SerializeField, ReadOnly] private float targetDistance;
    [SerializeField, ReadOnly] private float maxAngle = 30;
    #endregion
    #region AgentSpeed
    [SerializeField] protected float agentSprint;
    [SerializeField] protected float agentWalk;
    #endregion
    #region Detection
    [Header("Detection")]
    [SerializeField, ReadOnly] protected float detectionLevel = 0;
    protected float maxDetectionLevel = 100;
    [SerializeField] protected float detectionAmt = 15;
    [ReadOnly] public Transform target;
    [SerializeField] private Transform head;
    [SerializeField] private float lookSpeed;
    #endregion
    #region CanvasVariables
    //[SerializeField] protected Transform canvas;
    //[SerializeField] protected Image bar;
    #endregion
    #region Misc
    [SerializeField] protected LayerMask ignore;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected PlayerManager player;
    #endregion
    #region DebugVariables
    [InfoBox("Variables for changing the cone size for debugging purposes", EInfoBoxType.Error)]
    public bool _debug;
    [SerializeField, ShowIf("_debug")] private float lineDist;
    [SerializeField, ShowIf("_debug")] private float lineOffsetDist;
    #endregion
    #endregion

    #region Start and Update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerManager>();
    }

    protected virtual void Update()
    {
        DetectionCone();
        if (Debugging.debugMode){DebugDrawLines(); _debug = Debugging.debugMode;}
        else{_debug = false;}
        //canvas.LookAt(target);
        //DetectionBar(detectionLevel);
    }
    #endregion

    #region Handles Detection
    private void DetectionCone()
    {
        MathUtils.CalculateTargetAngle(head, target, out angle, out targetDistance);   // calculates the angle and distance
        RaycastHit _hit;

        // if the angle is less then x 
        if (((TargetAngle < MaxAngle) && (TargetDistance < 50f)) || (TargetAngle < 360f && TargetDistance <= 4.5f))
        {
            // send ray to the target to check if there is line of sight
            if (Physics.Linecast(head.position, target.position, out _hit))
            {
                if (_hit.collider.gameObject.CompareTag("Player")){ Debug.Log("can see you");                           // looks for the players collider 
                MathUtils.LookAtTarget(head, target.position, lookSpeed); player.DetectionMeterSpotted(detectionAmt);}  // if player is spotted increase detection
                else { player.DetectionMeterHidden(detectionAmt);}                                                      // otherwise decrease detection
            }
            Debug.DrawRay(head.position, target.position - head.position, Color.red);
        }
        else { player.DetectionMeterHidden(detectionAmt);}
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


    protected void EngageTarget()
    {
        agent.SetDestination(target.position);
        
        if (TargetDistance >= 30)
        {
            agent.speed = agentSprint;
        }
        else if (TargetDistance <= 25)
        {
            agent.speed = agentWalk;
        }
    }

    #region Debug
    private void OnDrawGizmos()
    {
        if (Debugging.debugMode){ Gizmos.DrawWireSphere(transform.position, 4); }
    }

    void DebugDrawLines()
    {
        float distance = lineDist;                              // used to make it go further out
        float offsetDistance = lineOffsetDist;                  // used to spread out the lines
        Vector3 offset = head.right * offsetDistance;      // sets the offset
        Vector3 forward = head.forward * distance;         // sets the direction

        Debug.DrawLine(head.position, head.position + forward - offset, Color.blue);   // left
        Debug.DrawLine(head.position, head.position + forward, Color.blue);            // middle
        Debug.DrawLine(head.position, head.position + forward + offset, Color.blue);   // right
    }
    #endregion
    #endregion
}
