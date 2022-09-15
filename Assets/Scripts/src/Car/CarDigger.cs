using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarDigger : MonoBehaviour
{
    [SerializeField] private LayerMask ChunkInteractMask;
    [SerializeField] private LayerMask BoundCheckMask;
    [SerializeField] private float InteractRange = 8f;
    private WorldGenerator WorldGenInstance;

    private void Start()
    {
        WorldGenInstance = FindObjectOfType<WorldGenerator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(camRay, out RaycastHit hitInfo, InteractRange, ChunkInteractMask))
            {
                Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;

                string chunkName = hitInfo.collider.gameObject.name;
                if (chunkName.Contains("Chunk"))
                {
                    WorldGenInstance.SetBlock(GetGridPoints(targetPoint).ToList(), 0);
                }
            }
        }
        
    }

    public HashSet<Vector3Int> GetGridPoints(Vector3 pos, float radius = 2f)
    {
        HashSet<Vector3Int> gridPoints = new HashSet<Vector3Int>();
        int radiusCeil = Mathf.CeilToInt(radius);
        for (int i = -radiusCeil; i <= radiusCeil; i++)
        {
            for (int j = -radiusCeil; j <= radiusCeil; j++)
            {
                for (int k = -radiusCeil; k <= radiusCeil; k++)
                {
                    Vector3 gridPoint = new Vector3(Mathf.Floor(pos.x + i),
                                                    Mathf.Floor(pos.y + j),
                                                    Mathf.Floor(pos.z + k));
                    if (Vector3.Distance(pos, gridPoint) <= radius)
                    {
                        gridPoints.Add(Vector3Int.FloorToInt(gridPoint));
                    }
                }
            }
        }
        return gridPoints;
    }

}