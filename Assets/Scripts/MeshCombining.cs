using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clean me up
public class MeshCombining : MonoBehaviour
{
    public bool CombineChildMeshes = false;

    public List<Block> Blocks = new List<Block>();
    public MeshFilter Filter;
    public MeshRenderer Rnder;
    public MeshCollider MeshColl;

    void Start()
    {
        Filter = transform.GetComponent<MeshFilter>();
        Rnder = transform.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (CombineChildMeshes)
        {
            CombineChildMeshes = false;
            CombineMeshes(GetComponentsInChildren<Block>());

            //Add collider to mesh (if needed)
            MeshColl = gameObject.AddComponent<MeshCollider>();
        }
    }

    public void CombineMeshes(Block[] blocks)
    {
        ArrayList materials = new ArrayList();
        ArrayList combineInstanceArrays = new ArrayList();
        //MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (Block block in blocks)
        {
            MeshRenderer meshRenderer = block.GetComponent<MeshRenderer>();

            if (!meshRenderer ||
                !block.SharedMeshRef ||
                meshRenderer.sharedMaterials.Length != block.SharedMeshRef.subMeshCount)
            {
                continue;
            }

            for (int s = 0; s < block.SharedMeshRef.subMeshCount; s++)
            {
                int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
                if (materialArrayIndex == -1)
                {
                    materials.Add(meshRenderer.sharedMaterials[s]);
                    materialArrayIndex = materials.Count - 1;
                }
                combineInstanceArrays.Add(new ArrayList());

                CombineInstance combineInstance = new CombineInstance();
                combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
                combineInstance.subMeshIndex = s;
                combineInstance.mesh = block.SharedMeshRef;
                (combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
            }
        }

        // Get / Create mesh filter & renderer
        MeshFilter meshFilterCombine = gameObject.GetComponent<MeshFilter>();
        if (meshFilterCombine == null)
        {
            meshFilterCombine = gameObject.AddComponent<MeshFilter>();
        }
        MeshRenderer meshRendererCombine = gameObject.GetComponent<MeshRenderer>();
        if (meshRendererCombine == null)
        {
            meshRendererCombine = gameObject.AddComponent<MeshRenderer>();
        }

        // Combine by material index into per-material meshes
        // also, Create CombineInstance array for next step
        Mesh[] meshes = new Mesh[materials.Count];
        CombineInstance[] combineInstances = new CombineInstance[materials.Count];

        for (int m = 0; m < materials.Count; m++)
        {
            CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
            meshes[m] = new Mesh();
            meshes[m].indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            meshes[m].CombineMeshes(combineInstanceArray, true, true);

            combineInstances[m] = new CombineInstance();
            combineInstances[m].mesh = meshes[m];
            combineInstances[m].subMeshIndex = 0;
        }

        // Combine into one
        meshFilterCombine.sharedMesh = new Mesh();
        meshFilterCombine.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);

        // Destroy other meshes
        foreach (Mesh oldMesh in meshes)
        {
            oldMesh.Clear();
            DestroyImmediate(oldMesh);
        }

        // Assign materials
        Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
        meshRendererCombine.materials = materialsArray;

        foreach (Block block in blocks)
        {
            DestroyImmediate(block.gameObject);
        }

    }

    private int Contains(ArrayList searchList, string searchName)
    {
        for (int i = 0; i < searchList.Count; i++)
        {
            if (((Material)searchList[i]).name == searchName)
            {
                return i;
            }
        }
        return -1;
    }
}
