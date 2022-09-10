using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public Vector3Int WorldPosition = new Vector3Int(0, 0, 0);
    public BlockType BlockType { get; set; }
    public Mesh SharedMeshRef;
    
    protected virtual void Awake() {
        BlockType = BlockType.BLOCK;
        SharedMeshRef = GetComponent<MeshFilter>().sharedMesh;
    }
}
