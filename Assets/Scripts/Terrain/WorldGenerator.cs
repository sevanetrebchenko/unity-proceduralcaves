﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int worldChunkSize = 10;

    Dictionary<Vector3Int, Chunk> worldChunks = new Dictionary<Vector3Int, Chunk>();

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        for (int x = 0; x < worldChunkSize; ++x)
        {
            for (int z = 0; z < worldChunkSize; ++z)
            {
                Vector3Int chunkPosition = new Vector3Int(x * ChunkData.chunkWidth, 0, z * ChunkData.chunkDepth);

                if (x > worldChunkSize / 2 || z > worldChunkSize / 2)
                {
                    worldChunks.Add(chunkPosition, new Chunk(chunkPosition, Chunk.TerrainType.SURFACE));
                }
                else
                {
                    worldChunks.Add(chunkPosition, new Chunk(chunkPosition, Chunk.TerrainType.CAVE));
                }

                worldChunks[chunkPosition].chunkObject.transform.SetParent(transform);
            }
        }
    }
}
