namespace VoxelEngine.Tests
{
	using System;
	using VoxelEngine.Jobs;
	using NUnit.Framework;
	using Unity.Collections;
	using Unity.Jobs;
	using Unity.Mathematics;
	using VoxelEngine.Core;

	public static class ChunkWorldTests
	{
		[Test]
		public static void GetChunk()
		{
			ChunkWorld chunkWorld = new(Allocator.Temp);

			/*//Index
			{
				Chunk chunk = chunkWorld.GetChunk(0);
				Assert.IsTrue(chunk.IsCreated);
			
				//Invalid coordinates.
				Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(-1, 0));
				Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(0, -1));
			}*/
			
			//2D Coordinates
			{
				ref Chunk chunk = ref chunkWorld.GetChunk(0, 0);
				Assert.AreEqual(new int2(0, 0), chunk.LocalPosition);
				
				chunk = ref chunkWorld.GetChunk(ChunkWorld.Width - 1, 0);
				Assert.AreEqual(new int2(ChunkWorld.Width - 1, 0), chunk.LocalPosition);
				
				chunk = ref chunkWorld.GetChunk(0, ChunkWorld.Length - 1);
				Assert.AreEqual(new int2(0, ChunkWorld.Length - 1), chunk.LocalPosition);
				
				chunk = ref chunkWorld.GetChunk(ChunkWorld.Width - 1, ChunkWorld.Length - 1);
				Assert.AreEqual(new int2(ChunkWorld.Width - 1, ChunkWorld.Length - 1), chunk.LocalPosition);
			
				//Invalid coordinates.
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(-1, 0));
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(0, -1));
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(-1, -1));
				
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(ChunkWorld.Width, 0));
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(0, ChunkWorld.Length));
				//Assert.Throws<IndexOutOfRangeException>(() => chunkWorld.GetChunk(ChunkWorld.Width, ChunkWorld.Length));
			}

			chunkWorld.Dispose();
		}

		/*[Test]
		public static void GetBlock()
		{
			//Setup world
			ChunkWorld chunkWorld = new(Allocator.Temp);

			InitializeBlocksJob2 initJob = new()
			{
				blocks = chunkWorld.chunks[0].blocks,
			};
			JobHandle initJobHandle = initJob.Schedule();
			initJobHandle.Complete();

			//Stone
			{
				FillChunkJob2 fillJob = new()
				{
					blocks = chunkWorld.chunks[0].blocks,
					blockType = BlockType.Stone,
				};
				JobHandle fillJobHandle = fillJob.Schedule();
				fillJobHandle.Complete();

				//Valid
				Block actualBlock = chunkWorld.GetBlock(0, 0);
				Assert.AreEqual(BlockType.Stone, actualBlock.blockType);
				Assert.AreEqual(new int3(0, 0, 0), actualBlock.position);
			}

			//Dirt
			{
				FillChunkJob2 fillJob = new()
				{
					blocks = chunkWorld.chunks[0].blocks,
					blockType = BlockType.Dirt,
				};
				JobHandle fillJobHandle = fillJob.Schedule();
				fillJobHandle.Complete();

				//Valid
				Block actualBlock = chunkWorld.GetBlock(0, 0);
				Assert.AreEqual(BlockType.Dirt, actualBlock.blockType);
				Assert.AreEqual(new int3(0, 0, 0), actualBlock.position);
			}

			chunkWorld.Dispose();
		}*/

		[Test]
		public static void GetChunkIndex()
		{
			//Valid
			Assert.AreEqual(0, ChunkWorld.GetChunkIndex(0, 0));
			Assert.AreEqual(ChunkWorld.Width - 1, ChunkWorld.GetChunkIndex(ChunkWorld.Width - 1, 0));
			Assert.AreEqual(0 + ((ChunkWorld.Length - 1) * ChunkWorld.Length), ChunkWorld.GetChunkIndex(0, ChunkWorld.Length - 1));
			Assert.AreEqual(ChunkWorld.TotalChunkCount - 1, ChunkWorld.GetChunkIndex(ChunkWorld.Width - 1, ChunkWorld.Length - 1));
		}
		
		[Test]
		public static void GetChunkPosition()
		{
			//Valid
			Assert.AreEqual(new int2(0, 0), ChunkWorld.GetChunkPosition(0, 0));
			Assert.AreEqual(new int2(1, 0), ChunkWorld.GetChunkPosition(Chunk.Width, 0));
			Assert.AreEqual(new int2(0, 1), ChunkWorld.GetChunkPosition(0, Chunk.Length));
			Assert.AreEqual(new int2(1, 1), ChunkWorld.GetChunkPosition(Chunk.Width, Chunk.Length));
			
			//Invalid
			Assert.AreEqual(new int2(-1, 0), ChunkWorld.GetChunkPosition(-1, 0));
		}

		#region Validation

		[Test]
		public static void IsValidPosition()
		{
			//Valid
			Assert.IsTrue(ChunkWorld.IsValidPosition(new int2(0, 0)));
			Assert.IsTrue(ChunkWorld.IsValidPosition(new int2((Chunk.Width * ChunkWorld.Width) - 1, 0)));
			Assert.IsTrue(ChunkWorld.IsValidPosition(new int2(0, (Chunk.Length * ChunkWorld.Length) - 1)));
			Assert.IsTrue(ChunkWorld.IsValidPosition(new int2(
				(Chunk.Width * ChunkWorld.Width) - 1, 
				(Chunk.Length * ChunkWorld.Length) - 1)));
			
			//Invalid
			Assert.IsFalse(ChunkWorld.IsValidPosition(new int2(-1, 0)));
			Assert.IsFalse(ChunkWorld.IsValidPosition(new int2(0, -1)));
    
			Assert.IsFalse(ChunkWorld.IsValidPosition(new int2(Chunk.Width * ChunkWorld.Width, 0)));
			Assert.IsFalse(ChunkWorld.IsValidPosition(new int2(0, Chunk.Length * ChunkWorld.Length)));
		}
		
		[Test]
		public static void IsValidChunkIndex()
		{
			//Valid
			Assert.IsTrue(ChunkWorld.IsValidChunkIndex(0));
			Assert.IsTrue(ChunkWorld.IsValidChunkIndex(ChunkWorld.TotalChunkCount - 1));
			
			//Invalid
			Assert.IsFalse(ChunkWorld.IsValidChunkIndex(-1));
			Assert.IsFalse(ChunkWorld.IsValidChunkIndex(ChunkWorld.TotalChunkCount));
		}

		#endregion
	}
}
