using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    [SerializeField] private Node Prefab;
    [SerializeField, Min(0.1f)] private float spacing = 1;
    [SerializeField] private Vector3Int size = Vector3Int.one * 20;


    // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
    private void OnValidate()
    {
        size.x = Mathf.Clamp(size.x, 1, 100);
        size.y = Mathf.Clamp(size.y, 1, 100);
        size.z = Mathf.Clamp(size.z, 1, 100);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3 position = new Vector3((x * spacing) + 0.5f, (y * spacing) + 0.5f, (z * spacing) + 0.5f);  
                    Node node = Instantiate(Prefab, position, Quaternion.identity, transform);

                    node.transform.localScale = Vector3.one * spacing;
                }
            }
        }  
    }
}
