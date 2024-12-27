namespace VoxelEngine.Utilities
{
	using Unity.Burst;
	using Unity.Collections;
	using VoxelEngine.Core;

	[BurstCompile]
	public static class ChunkBuilderUtils
	{
		/// <summary>
		/// Builds a single chunk in <see cref="chunkWorld"/>.
		/// </summary>
		/// <param name="chunkWorld">The <see cref="ChunkWorld"/> to modify.</param>
		/// <param name="chunkIndex">The index of the chunk in <see cref="chunkWorld"/>.</param>
		/// <param name="blockMapping">Block mapping definitions for block data.</param>
		[BurstCompile]
		public static void BuildChunk(
			//Output
			ref ChunkWorld chunkWorld,
			//Input
			int chunkIndex,
			ref NativeHashMap<BlockTypeEquatable, BlockDefinition> blockMapping)
		{
			ref Chunk chunk = ref chunkWorld.GetChunk(chunkIndex);
			
			//Clear all vertex data
			chunk.ClearVertexData();
			
			//Used to keep track of the correct index in the loop.
			int vertexIndex = 0;
			int colliderVertexIndex = 0;
				
			for(int i = 0; i < chunk.blocks.Length; i++)
			{
				//Current block
				Block block = chunk.blocks[i];
				
				//Skip building if block is empty
				if(block.blockType == BlockType.Empty) continue;

				//Check if there is a solid block in all six adjacent directions.
				//If so, remove building that face in that direction.
				IncludeFaces includeFaces = IncludeFaces.All;
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Forward, block.position))
					includeFaces &= ~IncludeFaces.Front;
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Backward, block.position))
					includeFaces &= ~IncludeFaces.Back;
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Left, block.position))
					includeFaces &= ~IncludeFaces.Left;
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Right, block.position))
					includeFaces &= ~IncludeFaces.Right;
				//Uses a different method. Always build the top most block and down most block since there are no chunks above or below. 
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Up, block.position))
					includeFaces &= ~IncludeFaces.Up;
				if(chunk.CheckAdjacentBlockIsSolid(Direction.Down, block.position))
					includeFaces &= ~IncludeFaces.Down;
				
				//Skip if not building any faces.
				if(includeFaces == IncludeFaces.None) continue;
				
				//Build block mesh
				CubeBuilderUtils.BuildCube_24Verts(ref chunk.meshVertexData, ref vertexIndex,
					block, ref blockMapping, includeFaces);

				//Build mesh collider
				CubeBuilderUtils.BuildCube_24Verts(ref chunk.colliderVertexData, ref colliderVertexIndex,
					block, includeFaces);
			}
		}
	}
}
