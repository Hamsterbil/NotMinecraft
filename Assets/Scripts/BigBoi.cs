using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BigBoi : GameManager
{
    public int Width = 50;
    public float Scale = 5f;

    protected override void StartUp(){
        noisyLandscape();
 
        //Generate grass
        Dictionary<Vector3Int, GameObject> grass = new Dictionary<Vector3Int, GameObject>();
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {
            Vector3Int grassHeight = key + new Vector3Int(0,1,0);
            grass.Add(grassHeight, Instantiate(Grass, grassHeight, Quaternion.identity, transform));
        }
        
        //Generate dirt
        Dictionary<Vector3Int, GameObject> dirt = new Dictionary<Vector3Int, GameObject>();
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {
            for(int i = -1; i > -5; i--){
                Vector3Int dirtHeight = key + new Vector3Int(0,i,0);
                dirt.Add(dirtHeight, Instantiate(Dirt, dirtHeight, Quaternion.identity, transform));
            }
        }

        //Generate gold
        Dictionary<Vector3Int, GameObject> gold = new Dictionary<Vector3Int, GameObject>();
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {

        }
        Blocks.Union(grass).Union(dirt).Union(gold);

    }

    private void noisyLandscape(){
        for(int x = 0; x < Width; x++){
            for(int z = 0; z < Width; z++){
                float noiseHeight = Mathf.PerlinNoise((float)x/Width*Scale,(float)z/Width*Scale)*10;
                Vector3Int blocksEntry = new Vector3Int(x,(int)noiseHeight,z);
                if(!Blocks.ContainsKey(blocksEntry)){
                    Blocks.Add(blocksEntry, Instantiate(Dirt, new Vector3(x,blocksEntry.y,z), Quaternion.identity, transform));
                }
            }
        }
    }
}
