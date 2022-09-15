using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChunkInteracter : MonoBehaviour
{
    [SerializeField] private LayerMask ChunkInteractMask;
    [SerializeField] private LayerMask BoundCheckMask;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private float InteractRange = 8f;
    private WorldGenerator WorldGenInstance;

    private void Start()
    {
        WorldGenInstance = FindObjectOfType<WorldGenerator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
            if (Physics.Raycast(camRay, out RaycastHit hitInfo, InteractRange, ChunkInteractMask))
            {
                Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;
        
                Vector3Int targetBlock = new Vector3Int
                {
                    x = Mathf.RoundToInt(targetPoint.x),
                    y = Mathf.RoundToInt(targetPoint.y),
                    z = Mathf.RoundToInt(targetPoint.z)
                };


                string chunkName = hitInfo.collider.gameObject.name;
                if (chunkName.Contains("Chunk"))
                {
                    WorldGenInstance.SetBlock(GetGridPoints(targetPoint).ToList(), 0);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray camRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
            if (Physics.Raycast(camRay, out RaycastHit hitInfo, 4f, ChunkInteractMask))
            {
                Vector3 targetPoint = hitInfo.point + hitInfo.normal * .1f;
                Vector3Int targetBlock = new Vector3Int
                {
                    x = Mathf.RoundToInt(targetPoint.x),
                    y = Mathf.RoundToInt(targetPoint.y),
                    z = Mathf.RoundToInt(targetPoint.z)
                };

                if (!Physics.CheckBox(targetBlock, Vector3.one * .5f, Quaternion.identity, BoundCheckMask))
                {
                    string chunkName = hitInfo.collider.gameObject.name;
                    if (chunkName.Contains("Chunk"))
                    {
                        WorldGenInstance.SetBlock(targetBlock, 2);
                    }
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