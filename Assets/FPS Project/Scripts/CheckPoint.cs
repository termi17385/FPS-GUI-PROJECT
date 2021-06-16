using FPSProject.Keybinds;
using FPSProject.Player;
using FPSProject.Player.Manager;
using FPSProject.Saving;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CharacterDataManager charManager;
    
    [SerializeField] private GameObject pressEToSave;
    [SerializeField] private bool inRange;

    public Transform playerTransform;
    public Vector2 playerRotation;

    private void Awake()
    {
        charManager = FindObjectOfType<CharacterDataManager>();
    }
    
    private void Update() => SavePlayerPosition();
    private void SavePlayerPosition()
    {
        if (BindingManager.BindingPressed("Interact") && inRange)
        {
            charManager.SaveData(this);
        }
    }
    
    // handles checking if the player is in range of the checkpoint
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var pTransform = other.transform;
            var pController = other.GetComponent<PlayerController>();
            
            // sets the transform and rotation
            playerTransform = pTransform;
            playerRotation = pController.rotation;
            
            pressEToSave.SetActive((inRange = true));
        }
    }
    private void OnTriggerExit(Collider other) => pressEToSave.SetActive((inRange = !other.CompareTag("Player")));
}
