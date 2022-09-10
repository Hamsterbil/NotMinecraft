using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Inherits from the GameManager.cs script, to populate the Block Dictionary with prefabs
//It makes use of Mathf.PerlinNoise(x,y) to generate a coresponding height value
public class BigBoi : GameManager
{
    //Perlion noise area
    public int Width = 15;
    public float Scale = 5f;

    //Override StartUp() method from GameManager
    protected override void StartUp()
    {
        noisyLandscape();

        //Generate grass
        Dictionary<Vector3Int, GameObject> grass = new Dictionary<Vector3Int, GameObject>();
        //Instantiate 1 layer of grass on top of the noisyLandscape()
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {
            Vector3Int grassHeight = key + new Vector3Int(0, 1, 0); 
            grass.Add(grassHeight, Instantiate(Grass, grassHeight, Quaternion.identity, transform));
        }

        //Generate dirt
        Dictionary<Vector3Int, GameObject> dirt = new Dictionary<Vector3Int, GameObject>();
        //Generate 5 levels of dirt below the noiseLandscape()
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {
            for (int i = -1; i > -5; i--)
            {
                Vector3Int dirtHeight = key + new Vector3Int(0, i, 0);
                dirt.Add(dirtHeight, Instantiate(Dirt, dirtHeight, Quaternion.identity, transform));
            }
        }

        //Generate gold
        Dictionary<Vector3Int, GameObject> gold = new Dictionary<Vector3Int, GameObject>();
        foreach (Vector3Int key in Blocks.Keys.ToList())
        {

        }

        //Joins the above created arrays
        Blocks.Union(grass).Union(dirt).Union(gold);
    }

    //Generates Dirt blocks in a xz-plane, and adjusts y-heights based on perlin noise
    private void noisyLandscape()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                float noiseHeight = Mathf.PerlinNoise((float)x / Width * Scale, (float)z / Width * Scale) * 10;
                Vector3Int blocksEntry = new Vector3Int(x, (int)noiseHeight, z);
                if (!Blocks.ContainsKey(blocksEntry))
                {
                    Blocks.Add(blocksEntry, Instantiate(Dirt, new Vector3(x, blocksEntry.y, z), Quaternion.identity, transform));
                }
            }
        }
    }
}