namespace VoxelEngine.Jobs
{
	using VoxelEngine.Core;
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Jobs;
	using VoxelEngine.Utilities;

	/// <summary>
	/// Builds the vertex data for a chunk.
	/// </summary>
	[BurstCompile]
	public struct BuildChunkMeshJob : IJob
	{
		//Output
		/// <summary>
		/// The chunk to build.
		/// </summary>
		public ChunkWorld chunkWorld;

		//Input
		public int chunkIndex;
		[ReadOnly]
		public NativeHashMap<BlockTypeEquatable, BlockDefinition> blockMapping;

		public void Execute()
		{
			ChunkBuilderUtils.BuildChunk(ref chunkWorld, chunkIndex, ref blockMapping);
		}
	}
}
