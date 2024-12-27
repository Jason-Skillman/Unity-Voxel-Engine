namespace VoxelEngine.Core
{
	using Unity.Mathematics;
	using UnityEngine;

	public class ChunkBehaviour : MonoBehaviour
	{
		[SerializeField]
		private MeshFilter meshFilter;
		[SerializeField]
		private MeshCollider meshCollider;

		public void Initialize(in Chunk chunk)
		{
			int2 worldChunkPosition = chunk.GetWorldPosition;

			transform.localPosition = new Vector3(worldChunkPosition.x, 0.0f, worldChunkPosition.y);
		}

		public void SetMesh(Mesh mesh)
		{
			meshFilter.mesh = mesh;
		}
		
		public void SetColliderMesh(Mesh mesh)
		{
			meshCollider.sharedMesh = mesh;
		}
	}
}
