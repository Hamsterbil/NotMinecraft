using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Grass;
    public GameObject Dirt;
    private Vector3Int _initialWorldSize = new Vector3Int(10,10,10);
    public Dictionary<Vector3Int, GameObject> Blocks = new Dictionary<Vector3Int, GameObject>();
    private int _playerSpawnGrassRange = 25;
    private int _playerSpawnBelow = 10;
    private int _blockSpawnTime = 1;

    public static GameManager Instance { get; private set; }
    
    void Awake(){
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        //Spawn top level grass blocks
        for(int x = -_initialWorldSize.x; x < _initialWorldSize.x; x++){
            for(int z = -_initialWorldSize.z; z < _initialWorldSize.z; z++){
                Blocks.Add(new Vector3Int(x,0,z), Instantiate(Grass, new Vector3(x,0,z), Quaternion.identity, transform));
            }   
        }
        //Spawn below dirt blocks
        for(int x = -_initialWorldSize.x; x < _initialWorldSize.x; x++){
            for(int y = -_initialWorldSize.y*2; y < 0; y++){
                for(int z = -_initialWorldSize.z; z < _initialWorldSize.z; z++){
                    Blocks.Add(new Vector3Int(x,y,z), Instantiate(Dirt, new Vector3(x,y,z), Quaternion.identity, transform));
                }
            }
        }
        StartCoroutine(SpawnNewBlocks(_blockSpawnTime));
    }

    private IEnumerator SpawnNewBlocks(float waitTime)
    {
        while (true)
        {
            Vector3Int playerLocation = Vector3Int.FloorToInt(Player.transform.position);

            for(int x = playerLocation.x-_playerSpawnGrassRange; x < playerLocation.x+_playerSpawnGrassRange; x++){
                for(int z = playerLocation.z-_playerSpawnGrassRange; z < playerLocation.z+_playerSpawnGrassRange; z++){
                    if(!Blocks.ContainsKey(new Vector3Int(x,0,z))) {
                        Blocks.Add(new Vector3Int(x,0,z), Instantiate(Grass, new Vector3(x,0,z), Quaternion.identity, transform));
                    }
                }
            }

            for(int x = playerLocation.x-_playerSpawnBelow; x < playerLocation.x+_playerSpawnBelow; x++){
                for(int y = playerLocation.y-_playerSpawnBelow; y < 0; y++){
                    for(int z = playerLocation.z-_playerSpawnBelow; z < playerLocation.z+_playerSpawnBelow; z++){
                        if(!Blocks.ContainsKey(new Vector3Int(x,y,z))) {
                            Blocks.Add(new Vector3Int(x,y,z), Instantiate(Dirt, new Vector3(x,y,z), Quaternion.identity, transform));
                        } 
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
