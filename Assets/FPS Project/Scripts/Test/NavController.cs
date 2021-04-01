using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class NavController : MonoBehaviour
{
    [SerializeField] private List<NavMeshAgent> agents = new List<NavMeshAgent>();
    [SerializeField] private List<NavMeshModifierVolume> waterVolumes = new List<NavMeshModifierVolume>();
    [SerializeField] private NavMeshSurface robotSurface;

    [SerializeField] private NavMeshAgent[] prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject parent;

    Vector3 pos;

    [SerializeField] float[] spawnOffsetX;
    [SerializeField] float[] spawnOffsetZ;

    [SerializeField] float offsetX = 1;
    [SerializeField] float offsetZ = 1;

    private new Camera camera;
    
    // Start is called before the first frame update
    void Start() => camera = gameObject.GetComponent<Camera>();

    // Update is called once per frame
    void Update()
    {
        pos.x = spawnPoint.position.x * offsetX;
        pos.y = spawnPoint.position.y * 1;
        pos.z = spawnPoint.position.z * offsetZ;

        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                agents.ForEach(agent => agent.SetDestination(hit.point));
            }
        }  

        if (Input.GetKeyDown(KeyCode.Space))
        {
            waterVolumes.ForEach(vols => vols.enabled = !vols.enabled);

            robotSurface.BuildNavMesh();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < 5; i++)
            {
                //spawn a set of agents at different offsets
                offsetX = spawnOffsetX[Random.Range(0, 5)];
                offsetZ = spawnOffsetZ[Random.Range(0, 5)];

                pos.x = spawnPoint.position.x * offsetX;
                pos.y = spawnPoint.position.y * 1;
                pos.z = spawnPoint.position.z * offsetZ;

                agents.Add(Instantiate(prefab[Random.Range(0,2)], pos, Quaternion.identity, parent.transform));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pos, 2);
        Gizmos.color = Color.red;
    }
}
