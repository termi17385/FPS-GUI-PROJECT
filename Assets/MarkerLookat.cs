using UnityEngine;
using FPSProject.Player;

public class MarkerLookat : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float dampening;
    // Start is called before the first frame update
    void Start() => player = FindObjectOfType<PlayerController>().transform;
    // Update is called once per frame
    void Update() => LookAtPlayer();
    /// <summary>
    /// Rotates the quest marker to look at the player
    /// </summary>
    private void LookAtPlayer()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampening);
    }
}
