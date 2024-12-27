namespace VoxelEngine.Jobs
{
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Jobs;
	using VoxelEngine.Core;
	using VoxelEngine.Utilities;

	/*/// <summary>
	/// Builds the vertex data for all chunks in chunk world.
	/// </summary>
	[BurstCompile]
	public struct BuildChunkWorldJob : IJob
	{
		//Output
		/// <summary>
		/// The chunk to build.
		/// </summary>
		public ChunkWorld chunkWorld;

		//Input
		[ReadOnly]
		public NativeHashMap<BlockTypeEquatable, BlockDefinition> blockMapping;

		public void Execute()
		{
			for(int chunkIndex = 0; chunkIndex < chunkWorld.chunks.Length; chunkIndex++)
			{
				ChunkBuilderUtils.BuildChunk(ref chunkWorld, chunkIndex, ref blockMapping);
			}
		}
	}*/
	
	/// <summary>
	/// Builds the vertex data for all chunks in chunk world.
	/// </summary>
	[BurstCompile]
	public struct BuildChunkWorldJob : IJobParallelFor
	{
		//Output
		/// <summary>
		/// The chunk to build.
		/// </summary>
		public ChunkWorld chunkWorld;

		//Input
		[ReadOnly]
		public NativeHashMap<BlockTypeEquatable, BlockDefinition> blockMapping;

		public void Execute(int chunkIndex)
		{
			ChunkBuilderUtils.BuildChunk(ref chunkWorld, chunkIndex, ref blockMapping);
		}
	}
}
