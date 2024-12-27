namespace VoxelEngine
{
	using System;
	using Unity.Collections;
	using Unity.Collections.LowLevel.Unsafe;
	using Unity.Mathematics;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.Rendering;
	using VoxelEngine.Core;

	public class Driver : MonoBehaviour
	{
		private struct VertexData
		{
			public float3 position;
			public float3 normals;
			public float2 uv0;
		}
		
		[SerializeField]
		private MeshFilter meshFilter;

		private NativeArray<VertexData> vertexData;
		private NativeArray<VertexAttributeDescriptor> vertexAttributeDescriptors;
		private NativeArray<int> indices;

		private Chunk chunk;

		private void Awake()
		{
			{
				vertexData = new NativeArray<VertexData>(8, Allocator.Persistent, NativeArrayOptions.ClearMemory);

				float3 face1 = new(-1, -1, -1);
				vertexData[0] = new VertexData()
				{
					position = face1,
					normals = math.normalize(face1),
					uv0 = new float2(0, 0),
				};
				float3 face2 = new float3(1, -1, -1);
				vertexData[1] = new VertexData()
				{
					position = face2,
					normals = math.normalize(face2),
					uv0 = new float2(1, 0),
				};
				float3 face3 = new float3(1, 1, -1);
				vertexData[2] = new VertexData()
				{
					position = face3,
					normals = math.normalize(face3),
					uv0 = new float2(1, 1),
				};
				float3 face4 = new float3(-1, 1, -1);
				vertexData[3] = new VertexData()
				{
					position = face4,
					normals = math.normalize(face4),
					uv0 = new float2(0, 1),
				};
				float3 face5 = new float3(-1, -1, 1);
				vertexData[4] = new VertexData()
				{
					position = face5,
					normals = math.normalize(face5),
					uv0 = new float2(0, 0),
				};
				float3 face6 = new float3(1, -1, 1);
				vertexData[5] = new VertexData()
				{
					position = face6,
					normals = math.normalize(face6),
					uv0 = new float2(1, 0),
				};
				float3 face7 = new float3(1, 1, 1);
				vertexData[6] = new VertexData()
				{
					position = face7,
					normals = math.normalize(face7),
					uv0 = new float2(1, 1),
				};
				float3 face8 = new float3(-1, 1, 1);
				vertexData[7] = new VertexData()
				{
					position = face8,
					normals = math.normalize(face8),
					uv0 = new float2(0, 1),
				};
			}
			
			vertexAttributeDescriptors = new NativeArray<VertexAttributeDescriptor>(3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			vertexAttributeDescriptors[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3);
			vertexAttributeDescriptors[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3);
			vertexAttributeDescriptors[2] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2);		//UV0

			{
				const int IndicesCount = 6 * 6;
				indices = new NativeArray<int>(IndicesCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			
				//Use array for ease of use
				int[] indicesArr = {
					// Front face
					0, 2, 1, 
					0, 3, 2, 
					// Back face
					4, 5, 6, 
					4, 6, 7, 
					// Left face
					0, 7, 3,
					0, 4, 7, 
					// Right face
					1, 2, 6, 
					1, 6, 5, 
					// Top face
					2, 3, 7, 
					2, 7, 6, 
					// Bottom face
					0, 1, 5, 
					0, 5, 4 
				};
				indices.CopyFrom(indicesArr);
			}
		}

		private void Start()
		{
			//Mesh
			/*{
				Mesh mesh = new();
			
				MeshUtils.UpdateMesh2(mesh, vertexData, vertexAttributeDescriptors, indices);
			
				meshFilter.mesh = mesh;
			}*/

			//Chunk
			{
				chunk = new Chunk(0, 0, Allocator.Persistent);

				unsafe
				{
					/*Block* blockPtr = (Block*)chunk.blocks.GetUnsafePtr();
				
					//Set the entire bottom of the chunk to stone
					for(int i = 0; i < Chunk.LayerCount; i++)
					{
						/*Block chunkBlock = chunk.blocks[i];
						chunkBlock.blockType = BlockType.Stone;
						chunk.blocks[i] = chunkBlock;#1#
					
						blockPtr[i].blockType = BlockType.Stone;
					}*/
				}
			}
		}

		private void OnDestroy()
		{
			vertexData.Dispose();
			vertexAttributeDescriptors.Dispose();
			indices.Dispose();
			
			chunk.Dispose();
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.color = Color.green;

			for(int i = 0; i < chunk.blocks.Length; i++)
			{
				Block block = chunk.blocks[i];
				float3 position = block.position;
				
				if(block.blockType == BlockType.Empty)
					continue;

				Vector3 positionVector3 = new(position.x, position.y, position.z);
				Handles.DrawWireCube(positionVector3, new Vector3(0.95f, 0.85f, 0.85f));
				Handles.Label(positionVector3, $"{i}");
			}
		}
#endif
	}
}
