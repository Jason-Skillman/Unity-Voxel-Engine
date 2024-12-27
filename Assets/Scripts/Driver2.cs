namespace VoxelEngine
{
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Jobs;
	using Unity.Mathematics;
	using UnityEngine;
	
	[BurstCompile]
	public struct GenerateCubeVerticesJob : IJob
	{
		public NativeArray<float3> vertices;
		public NativeArray<float3> normals;
		public NativeArray<float2> uvs;

		public void Execute()
		{
			// Define the vertices at half scale
			vertices[0] = new float3(-0.5f, -0.5f, -0.5f);
			vertices[1] = new float3(0.5f, -0.5f, -0.5f);
			vertices[2] = new float3(0.5f,  0.5f, -0.5f);
			vertices[3] = new float3(-0.5f,  0.5f, -0.5f);
			vertices[4] = new float3(-0.5f, -0.5f,  0.5f);
			vertices[5] = new float3(0.5f, -0.5f,  0.5f);
			vertices[6] = new float3(0.5f,  0.5f,  0.5f);
			vertices[7] = new float3(-0.5f,  0.5f,  0.5f);

			// Define normals (point outwards)
			for(int i = 0; i < 8; i++)
			{
				normals[i] = math.normalize(vertices[i]);
			}

			// Define UVs
			uvs[0] = new float2(0, 0);
			uvs[1] = new float2(1, 0);
			uvs[2] = new float2(1, 1);
			uvs[3] = new float2(0, 1);
			uvs[4] = new float2(0, 0);
			uvs[5] = new float2(1, 0);
			uvs[6] = new float2(1, 1);
			uvs[7] = new float2(0, 1);
		}
	}

	[BurstCompile]
	public struct GenerateCubeTrianglesJob : IJob
	{
		public NativeArray<int> triangles;

		public void Execute()
		{
			// Define the triangles
			triangles[0] = 0; triangles[1] = 2; triangles[2] = 1;
			triangles[3] = 0; triangles[4] = 3; triangles[5] = 2;
			triangles[6] = 4; triangles[7] = 5; triangles[8] = 6;
			triangles[9] = 4; triangles[10] = 6; triangles[11] = 7;
			triangles[12] = 0; triangles[13] = 7; triangles[14] = 3;
			triangles[15] = 0; triangles[16] = 4; triangles[17] = 7;
			triangles[18] = 1; triangles[19] = 2; triangles[20] = 6;
			triangles[21] = 1; triangles[22] = 6; triangles[23] = 5;
			triangles[24] = 2; triangles[25] = 3; triangles[26] = 7;
			triangles[27] = 2; triangles[28] = 7; triangles[29] = 6;
			triangles[30] = 0; triangles[31] = 1; triangles[32] = 5;
			triangles[33] = 0; triangles[34] = 5; triangles[35] = 4;
		}
	}

	public class Driver2 : MonoBehaviour
	{
		[SerializeField]
		private MeshFilter meshFilter;
		
		private NativeArray<float3> vertices;
		private NativeArray<int> triangles;
		private NativeArray<float3> normals;
		private NativeArray<float2> uvs;

		void Start()
		{
			// Allocate memory for the vertices, triangles, normals, and uvs
			vertices = new NativeArray<float3>(8, Allocator.TempJob);
			triangles = new NativeArray<int>(36, Allocator.TempJob);
			normals = new NativeArray<float3>(8, Allocator.TempJob);
			uvs = new NativeArray<float2>(8, Allocator.TempJob);

			// Create and schedule the jobs
			GenerateCubeVerticesJob verticesJob = new GenerateCubeVerticesJob
			{
				vertices = vertices,
				normals = normals,
				uvs = uvs
			};

			GenerateCubeTrianglesJob trianglesJob = new GenerateCubeTrianglesJob
			{
				triangles = triangles
			};

			JobHandle verticesHandle = verticesJob.Schedule();
			JobHandle trianglesHandle = trianglesJob.Schedule(verticesHandle);
			JobHandle.CompleteAll(ref verticesHandle, ref trianglesHandle);

			// Create the mesh
			Mesh mesh = new Mesh();
			mesh.vertices = vertices.Reinterpret<Vector3>().ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.normals = normals.Reinterpret<Vector3>().ToArray();
			mesh.uv = uvs.Reinterpret<Vector2>().ToArray();

			// Assign the mesh to a MeshFilter and MeshRenderer component
			meshFilter.mesh = mesh;

			// Dispose of the NativeArrays
			vertices.Dispose();
			triangles.Dispose();
			normals.Dispose();
			uvs.Dispose();
		}
	}

}
