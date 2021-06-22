using System.Collections;
using FPSProject.Keybinds;
using FPSProject.Player;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalItem : MonoBehaviour
{
    public Item item;
    [SerializeField] private Inventory inv;
    [SerializeField] private Transform player;

    [SerializeField] private GameObject pickupDisplay;
    [SerializeField] private float dampening;
    
    [SerializeField] RawImage itemDisplay;

    private void Start()
    {
        gameObject.name = item.name;
        itemDisplay.texture = item.Icon;
        
        inv = FindObjectOfType<Inventory>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (AllowPickup() && BindingManager.BindingPressed("Interact")) inv.SpawnInventoryItems(transform);
        pickupDisplay.SetActive(AllowPickup());
        
        LookAtPlayer(player);   
    }

    /// <summary>
    /// Destroys the object duh
    /// </summary>
    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Makes the item look at the player
    /// </summary>
    /// <param name="_target"></param>
    private void LookAtPlayer(Transform _target)
    {
        Vector3 lookPos = _target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampening);
    }
    /// <summary>
    /// Determines if the player is close enough to pick up item
    /// </summary>
    private bool AllowPickup()
    {
        return Vector2.Distance(transform.position, player.position) <= 2;
    }
}
