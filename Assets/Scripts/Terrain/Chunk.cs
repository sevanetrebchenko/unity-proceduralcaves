using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Chunk
{
	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int>();

	public GameObject chunkObject;
	Material chunkMaterial;
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;
	MeshCollider meshCollider;

	Vector3Int chunkPosition;

	[SerializeField]
	public bool smoothTerrain = false;

	public enum TerrainType
	{
		SURFACE,
		CAVE
	}
	[SerializeField]
	public TerrainType terrainType;

	[SerializeField]
	public bool flatShaded = false;

	float[,,] terrainMap;

	public Chunk(Vector3Int chunkPosition, TerrainType terrainType) {
		chunkObject = new GameObject();
		chunkObject.name = string.Format("Chunk: ({0}, {1})", chunkPosition.x, chunkPosition.y);
		this.chunkPosition = chunkPosition;
		chunkObject.transform.position = chunkPosition;

		this.terrainType = terrainType;
		chunkMaterial = new Material(Shader.Find("Standard"));

		// Add mesh filter.
		meshFilter = chunkObject.AddComponent<MeshFilter>();

		// Add mesh renderer.
		meshRenderer = chunkObject.AddComponent<MeshRenderer>();

		// Add mesh collider.
		meshCollider = chunkObject.AddComponent<MeshCollider>();


		meshFilter.GetComponent<MeshRenderer>().sharedMaterial = chunkMaterial;
		chunkObject.transform.tag = "Terrain";

		terrainMap = new float[ChunkData.chunkWidth + 1, ChunkData.chunkHeight + 1, ChunkData.chunkDepth + 1];
		PopulateTerrainMap();
		CreateMeshData();
		BuildMesh();
	}

	public void GenerateMesh()
    {
		terrainMap = new float[ChunkData.chunkWidth + 1, ChunkData.chunkHeight + 1, ChunkData.chunkDepth + 1];
		PopulateTerrainMap();
		CreateMeshData();
		BuildMesh();
	}

    private void PopulateTerrainMap()
    {
		for (int x = 0; x < ChunkData.chunkWidth + 1; ++x)
        {
			for (int y = 0; y < ChunkData.chunkHeight + 1; ++y)
            {
				for (int z = 0; z < ChunkData.chunkDepth + 1; ++z)
                {
					// Get height.
					float blockHeight = ChunkData.GetTerrainHeight(terrainType, x + chunkPosition.x, y + chunkPosition.y, z + chunkPosition.z);
					terrainMap[x, y, z] = (float)y - blockHeight;
                }
            }
        }
    }

	private int GetCubeConfiguration(float[] cube)
    {
		int configIndex = 0;
		for (int i = 0; i < 8; ++i)
        {
			// Above the surface
			if (cube[i] > ChunkData.terrainSurfaceLevel)
            {
				// Set config index;
				configIndex |= 1 << i;
            }
        }

		return configIndex;
    }

    private void MarchCube(Vector3Int cubePosition)
	{
		float[] cube = new float[8];
		for (int i = 0; i < 8; ++i)
		{
			cube[i] = SampleTerrain(cubePosition + ChunkData.cornerTable[i]);
		}

		int triangleConfigIndex = GetCubeConfiguration(cube);

		// Cube is completely in the air or completely in the ground.
		// No need to add anything.
		if (triangleConfigIndex == 0 || triangleConfigIndex == 255)
        {
			return;
        }

		int edgeIndex = 0;
		for (int i = 0; i < 5; ++i)
        {
			for (int j = 0; j < 3; ++j)
            {
				int index = ChunkData.triangleTable[triangleConfigIndex, edgeIndex];

				// Reached the end of this configuration.
				if (index == -1)
                {
					return;
                }

				Vector3 edgeVertex1 = cubePosition + ChunkData.cornerTable[ChunkData.edgeTable[index, 0]];
				Vector3 edgeVertex2 = cubePosition + ChunkData.cornerTable[ChunkData.edgeTable[index, 1]];
				Vector3 vertexPosition;

				// Terrain smoothing
				if (smoothTerrain)
                {
					float edgeVertex1Noise = cube[ChunkData.edgeTable[index, 0]];
					float edgeVertex2Noise = cube[ChunkData.edgeTable[index, 1]];

					float diff = edgeVertex2Noise - edgeVertex1Noise;

					if (diff == 0)
                    {
						diff = ChunkData.terrainSurfaceLevel;
                    }
					else
                    {
						diff = (ChunkData.terrainSurfaceLevel - edgeVertex1Noise) / diff;
                    }

					vertexPosition = edgeVertex1 + ((edgeVertex2 - edgeVertex1) * diff);
                }
				else
                {
					vertexPosition = (edgeVertex1 + edgeVertex2) / 2f;
				}

				// Flat shading
				if (!flatShaded)
                {
					vertices.Add(vertexPosition);
					triangles.Add(vertices.Count - 1);
				}
				else
                {
					triangles.Add(VertexForIndex(vertexPosition));
                }


				++edgeIndex;
			}
        }
    }

	float SampleTerrain(Vector3Int point)
    {
		return terrainMap[point.x, point.y, point.z];
    }

	int VertexForIndex(Vector3 vertex)
    {
		for (int i = 0; i < vertices.Count; ++i)
        {
			if (vertices[i] == vertex)
            {
				return i;
            }
        }

		vertices.Add(vertex);
		return vertices.Count - 1;
    }

	void ClearMeshData()
    {
		vertices.Clear();
		triangles.Clear();
    }

	void CreateMeshData()
	{
		ClearMeshData(); 

		for (int x = 0; x < ChunkData.chunkWidth; ++x)
		{
			for (int y = 0; y < ChunkData.chunkHeight; ++y)
			{
				for (int z = 0; z < ChunkData.chunkDepth; ++z)
				{
					MarchCube(new Vector3Int(x, y, z));
				}
			}
		}
	}

	void BuildMesh()
    {
		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();

		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}
}
