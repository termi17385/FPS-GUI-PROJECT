using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Node : MonoBehaviour
{
    [SerializeField] private Color safe = Color.green;
    [SerializeField] private Color ignored = Color.red;

    private new Rigidbody rigidbody;
    private new BoxCollider collider;
    private new MeshRenderer renderer;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        
        renderer = GetComponent<MeshRenderer>();
        renderer.material.color = safe;
    }

    #region Collision
    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider _other)
    {
        TryChangeColor(_other, ignored);
    }

    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    private void OnTriggerStay(Collider _other)
    {
        TryChangeColor(_other, ignored);
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    private void OnTriggerExit(Collider _other)
    {
        TryChangeColor(_other, safe);
    }
    #endregion

    private void TryChangeColor(Collider _other, Color _color)
    {
        if (_other.GetComponent<Node>() || !_other.CompareTag("PathFindingObstacle"))
        {
            return;
        }

        if (renderer == null)
        {
            renderer = gameObject.GetComponent<MeshRenderer>();
        }

        renderer.material.color = _color;
    }
}
