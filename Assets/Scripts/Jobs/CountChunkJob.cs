namespace VoxelEngine.Jobs
{
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Jobs;
	using Unity.Mathematics;
	using VoxelEngine.Core;

	[BurstCompile]
	public struct CountChunkJob : IJob
	{
		[ReadOnly]
		public Chunk chunk;

		private int blockCount;
		public int BlockCount => blockCount;

		public void Execute()
		{
			blockCount = 0;
			
			for(int y = 0; y < Chunk.Height; y++)
			{
				for(int z = 0; z < Chunk.Width; z++)
				{
					for(int x = 0; x < Chunk.Width; x++)
					{
						int index = Chunk.GetBlockIndex(x, y, z);
						Block block = chunk.blocks[index];

						if(block.blockType != BlockType.Empty)
						{
							blockCount++;
						}
					}
				}
			}
		}
	}
}
