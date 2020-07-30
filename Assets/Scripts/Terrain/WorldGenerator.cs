using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int worldChunkSize = 5;
    Dictionary<Vector3Int, Chunk> worldChunks = new Dictionary<Vector3Int, Chunk>();

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < worldChunkSize; ++x)
        {
            for (int z = 0; z < worldChunkSize; ++z)
            {
                Vector3Int chunkPosition = new Vector3Int(x * ChunkData.chunkWidth, 0, z * ChunkData.chunkDepth);
                if (!worldChunks.ContainsKey(chunkPosition))
                {
                    Debug.Log("Creating new chunk");
                    float chunkHeight = ChunkData.terrainHeightRange;

                    Chunk chunk = new Chunk(chunkPosition, Chunk.TerrainType.SURFACE);
                    chunk.chunkObject.transform.parent = transform;

                    worldChunks.Add(chunkPosition, chunk);

                    ChunkData.terrainHeightRange = chunkHeight;
                }
            }
        }
    }

    public void ClearChunks()
    {
        for (int x = 0; x < worldChunkSize; ++x)
        {
            for (int z = 0; z < worldChunkSize; ++z)
            {
                Vector3Int chunkPosition = new Vector3Int(x * ChunkData.chunkWidth, 0, z * ChunkData.chunkDepth);
                DestroyImmediate(worldChunks[chunkPosition].chunkObject);
                worldChunks.Remove(chunkPosition);
            }
        }
    }
}
