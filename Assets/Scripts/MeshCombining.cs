using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombining : MonoBehaviour
{
    public bool CombineChildMeshes = false;

    public List<Block> Blocks = new List<Block>();
    public MeshFilter Filter;
    public MeshRenderer Rnder;
    public MeshCollider MeshColl;

    void Start(){
        Filter = transform.GetComponent<MeshFilter>();
        Rnder = transform.GetComponent<MeshRenderer>();
    }

    void Update(){
        if(CombineChildMeshes){
            CombineChildMeshes = false;
            CombineMeshes();
        }
    }

    public void CombineMeshes()
    {
        //Temporarily set position to zero to make matrix math easier
        Vector3 position = transform.position;
        transform.position = Vector3.zero;

        //Get all mesh filters and combine
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 1;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        Filter.mesh = new Mesh();
        Filter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Filter.mesh.CombineMeshes(combine, true, true);
        Rnder.material = Rnder.materials[0];
        transform.gameObject.SetActive(true);

        //Return to original position
        transform.position = position;

        //Add collider to mesh (if needed)
        MeshColl = gameObject.AddComponent<MeshCollider>();
    }

}