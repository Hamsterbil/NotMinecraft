using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float count;
    private Vector2Int ChunkPosition = new Vector2Int(0,0);


    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            ChunkPosition.x = Mathf.FloorToInt(transform.position.x / WorldGenerator.ChunkSize.x);
            ChunkPosition.y = Mathf.FloorToInt(transform.position.z / WorldGenerator.ChunkSize.z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 20, 100, 25), "FPS: " + Mathf.Round(count));
        GUI.Label(new Rect(5, 40, 200, 25), "Chunk position: " + ChunkPosition);
        GUI.Label(new Rect(5, 60, 100, 25), "Cargo: 0 % full");
        GUI.Label(new Rect(5, 80, 100, 25), "Fuel: 0% left");
        GUI.Label(new Rect(5, 100, 100, 25), "Health: " + 100f);
    }
}
