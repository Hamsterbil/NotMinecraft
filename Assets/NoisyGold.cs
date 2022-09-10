using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyGold : GameManager
{
    public GameObject Gold;
    //Perlion noise area
    public int Width = 100;
    public float Scale = 5f;

    //Checks if blocks needs to be spawned within a certain range of the players position
    //Makes use of StartCoroutine() to create a method that is called once every x second
    protected override IEnumerator SpawnNewBlocks(float waitTime)
    {
        while (true)
        {
            //Translating the player coordinates to integer 
            Vector3Int playerLocation = Vector3Int.FloorToInt(_player.transform.position);
            
            //Creating grass
            for (int x = playerLocation.x - _playerSpawnGrassRange; x < playerLocation.x + _playerSpawnGrassRange; x++)
            {
                for (int z = playerLocation.z - _playerSpawnGrassRange; z < playerLocation.z + _playerSpawnGrassRange; z++)
                {
                    if (!Blocks.ContainsKey(new Vector3Int(x, 0, z)))
                    {
                        Blocks.Add(new Vector3Int(x, 0, z), Instantiate(Grass, new Vector3(x, 0, z), Quaternion.identity, transform));
                    }
                }
            }

            for (int x = playerLocation.x - _playerSpawnBelow; x < playerLocation.x + _playerSpawnBelow; x++)
            {
                for (int z = playerLocation.z - _playerSpawnBelow; z < playerLocation.z + _playerSpawnBelow; z++){
                    float noiseHeight = Mathf.PerlinNoise((float)x / Width * Scale, (float)z / Width * Scale) * 10;
                    Vector3Int asd = new Vector3Int(x, (int)noiseHeight-10, z);
                    if(noiseHeight > 7 && !Blocks.ContainsKey(asd)){
                        Debug.Log("Gold spawned " + asd);
                        Blocks.Add(asd, Instantiate(Gold, new Vector3(x, asd.y, z), Quaternion.identity, transform));
                    }
                }
            


                for (int y = playerLocation.y - _playerSpawnBelow; y < 0; y++)
                {
                    for (int z = playerLocation.z - _playerSpawnBelow; z < playerLocation.z + _playerSpawnBelow; z++)
                    {
                        if (!Blocks.ContainsKey(new Vector3Int(x, y, z)))
                        {
                            Blocks.Add(new Vector3Int(x, y, z), Instantiate(Dirt, new Vector3(x, y, z), Quaternion.identity, transform));               
                        }
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
