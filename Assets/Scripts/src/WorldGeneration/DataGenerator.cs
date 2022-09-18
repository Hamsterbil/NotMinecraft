using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class DataGenerator
{
    public class GenData
    {
        public System.Action<int[,,]> OnComplete;
        public Vector3Int GenerationPoint;
    }

    private WorldGenerator GeneratorInstance;
    private Queue<GenData> DataToGenerate;
    private Vector2 _noiseScale = new Vector2(1f, 0.1f);

    public bool Terminate;
    public DataGenerator(WorldGenerator worldGen)
    {
        GeneratorInstance = worldGen;
        DataToGenerate = new Queue<GenData>();

        worldGen.StartCoroutine(DataGenLoop());
    }

    public void QueueDataToGenerate(GenData data)
    {
        DataToGenerate.Enqueue(data);
    }

    public IEnumerator DataGenLoop()
    {
        while (Terminate == false)
        {
            if (DataToGenerate.Count > 0)
            {
                GenData gen = DataToGenerate.Dequeue();
                yield return GeneratorInstance.StartCoroutine(GenerateData(gen.GenerationPoint, gen.OnComplete));
            }

            yield return null;
        }
    }

    public IEnumerator GenerateData(Vector3Int offset, System.Action<int[,,]> callback)
    {
        Vector3Int ChunkSize = WorldGenerator.ChunkSize;

        int[,,] TempData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        Task t = Task.Factory.StartNew(delegate
        {
            if (offset.x < 2 && offset.z < 2 && offset.x > -2 && offset.z > -2)
            {
                TempData = generateStartingArea(offset);
            }
            else if (offset.x > 2 && offset.z > 2)
            {
                TempData = generateMountains(offset);
            }
            else
            {
                TempData = generatePlains(offset);
            }
        });

        yield return new WaitUntil(() =>
        {
            return t.IsCompleted || t.IsCanceled;
        });

        if (t.Exception != null)
            Debug.LogError(t.Exception);

        WorldGenerator.WorldData.Add(offset, TempData);
        callback(TempData);
    }

    private int[,,] generateStartingArea(Vector3Int offset)
    {
        Vector3Int ChunkSize = WorldGenerator.ChunkSize;
        Vector2 NoiseOffset = GeneratorInstance.NoiseOffset;
        Vector2 NoiseScale = GeneratorInstance.NoiseScale;

        float HeightIntensity = GeneratorInstance.HeightIntensity;
        float HeightOffset = GeneratorInstance.HeightOffset;

        int[,,] TempData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        for (int x = 0; x < ChunkSize.x; x++)
        {
            for (int z = 0; z < ChunkSize.z; z++)
            {
                //float PerlinCoordX = NoiseOffset.x + (x + (offset.x * 16f)) / ChunkSize.x * NoiseScale.x;
                //float PerlinCoordY = NoiseOffset.y + (z + (offset.z * 16f)) / ChunkSize.z * NoiseScale.y;
                //int HeightGen = Mathf.RoundToInt(Mathf.PerlinNoise(PerlinCoordX, PerlinCoordY) * HeightIntensity + HeightOffset);
                int HeightGen = Mathf.RoundToInt(HeightOffset);
                for (int y = HeightGen; y >= 0; y--)
                {
                    int BlockTypeToAssign = 0;

                    // Set first layer to grass
                    if (y == HeightGen) BlockTypeToAssign = 1;

                    //Set next 3 layers to dirt
                    if (y < HeightGen && y > HeightGen - 4) BlockTypeToAssign = 2;

                    //Set everything between the dirt range (inclusive) and 0 (exclusive) to stone
                    if (y <= HeightGen - 4 && y > 0) BlockTypeToAssign = 3;

                    //Set everything at height 0 to bedrock.
                    if (y == 0) BlockTypeToAssign = 4;

                    TempData[x, y, z] = BlockTypeToAssign;
                }
            }
        }
        return TempData;
    }

    private int[,,] generatePlains(Vector3Int offset)
    {
        Vector3Int ChunkSize = WorldGenerator.ChunkSize;
        Vector2 NoiseOffset = GeneratorInstance.NoiseOffset;
        Vector2 NoiseScale = GeneratorInstance.NoiseScale;

        float HeightIntensity = GeneratorInstance.HeightIntensity;
        float HeightOffset = GeneratorInstance.HeightOffset;

        int[,,] TempData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        for (int x = 0; x < ChunkSize.x; x++)
        {
            for (int z = 0; z < ChunkSize.z; z++)
            {
                float PerlinCoordX = NoiseOffset.x + (x + (offset.x * 16f)) / ChunkSize.x * NoiseScale.x;
                float PerlinCoordY = NoiseOffset.y + (z + (offset.z * 16f)) / ChunkSize.z * NoiseScale.y;
                int HeightGen = Mathf.RoundToInt(Mathf.PerlinNoise(PerlinCoordX, PerlinCoordY) * HeightIntensity + HeightOffset);

                for (int y = HeightGen; y >= 0; y--)
                {
                    int BlockTypeToAssign = 0;

                    // Set first layer to grass
                    if (y == HeightGen) BlockTypeToAssign = 1;

                    //Set next 3 layers to dirt
                    if (y < HeightGen && y > HeightGen - 4) BlockTypeToAssign = 2;

                    //Set everything between the dirt range (inclusive) and 0 (exclusive) to stone
                    if (y <= HeightGen - 4 && y > 0) BlockTypeToAssign = 3;

                    //Set everything at height 0 to bedrock.
                    if (y == 0) BlockTypeToAssign = 4;

                    TempData[x, y, z] = BlockTypeToAssign;
                }

            }
        }
        return TempData;
    }

    private int[,,] generateMountains(Vector3Int offset)
    {
        Vector3Int ChunkSize = WorldGenerator.ChunkSize;
        Vector2 NoiseOffset = GeneratorInstance.NoiseOffset;
        Vector2 NoiseScale = GeneratorInstance.NoiseScale;

        float HeightIntensity = GeneratorInstance.HeightIntensity;
        float HeightOffset = GeneratorInstance.HeightOffset;


        float increase = (offset.x + offset.z) / 2;
        if (increase >= HeightOffset)
        {
            increase = HeightOffset;
        }
        HeightIntensity = increase - 1;

        int[,,] TempData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        for (int x = 0; x < ChunkSize.x; x++)
        {
            for (int z = 0; z < ChunkSize.z; z++)
            {
                float PerlinCoordX = NoiseOffset.x + (x + (offset.x * 16f)) / ChunkSize.x * NoiseScale.x;
                float PerlinCoordY = NoiseOffset.y + (z + (offset.z * 16f)) / ChunkSize.z * NoiseScale.y;
                int HeightGen = Mathf.RoundToInt(Mathf.PerlinNoise(PerlinCoordX, PerlinCoordY) * HeightIntensity + HeightOffset);

                for (int y = HeightGen; y >= 0; y--)
                {
                    int BlockTypeToAssign = 0;

                    // Set first layer to grass
                    if (y == HeightGen) BlockTypeToAssign = 1;

                    //Set next 3 layers to dirt
                    if (y < HeightGen && y > HeightGen - 4) BlockTypeToAssign = 2;

                    //Set everything between the dirt range (inclusive) and 0 (exclusive) to stone
                    if (y <= HeightGen - 4 && y > 0) BlockTypeToAssign = 3;

                    //Set everything at height 0 to bedrock.
                    if (y == 0) BlockTypeToAssign = 4;

                    float p3Offset = NoiseScale.x;
                    float px = p3Offset + (PerlinCoordX * _noiseScale.x);
                    float py = p3Offset + (y * _noiseScale.y);
                    float pz = p3Offset + (PerlinCoordY * _noiseScale.x);

                    float noiseValue = Perlin3D(px, py, pz);
                    if (noiseValue >= 0.52f && y < HeightGen - 5)
                    {
                        BlockTypeToAssign = 5;
                    }

                    TempData[x, y, z] = BlockTypeToAssign;
                }
            }
        }
        return TempData;
    }

    public static float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;
    }


}