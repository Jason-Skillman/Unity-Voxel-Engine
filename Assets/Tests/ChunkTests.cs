namespace VoxelEngine.Tests
{
	using Unity.Collections;
	using Unity.Mathematics;
	using VoxelEngine;
	using VoxelEngine.Utilities;
	using NUnit.Framework;
	using UnityEngine;
	using VoxelEngine.Core;

	public static class ChunkTests
	{
		[Test]
		public static void GetBlock()
		{
			Chunk chunk = new(0, 0, Allocator.Temp);

			Block expectedBlock = new()
			{
				blockType = BlockType.Stone
			};

			//Fill
			for(int i = 0; i < chunk.blocks.Length; i++)
			{
				chunk.blocks[i] = expectedBlock;
			}

			//Test
			for(int y = 0; y < Chunk.Height; y++)
			for(int z = 0; z < Chunk.Width; z++)
			for(int x = 0; x < Chunk.Width; x++)
			{
				//int3 position = new int3(0, 0, 0);
				Block actualBlock = chunk.GetBlock(x, y, z);
				Assert.AreEqual(expectedBlock, actualBlock);
			}

			chunk.Dispose();
		}
		
		[Test]
		public static void GetBlockIndex()
		{
			int index = Chunk.GetBlockIndex(new int3(1, 0, 0));
			Assert.AreEqual(1, index);

			index = Chunk.GetBlockIndex(new int3(0, 1, 0));
			Assert.AreEqual(Chunk.Width * Chunk.Width, index);

			index = Chunk.GetBlockIndex(new int3(0, 0, 1));
			Assert.AreEqual(Chunk.Width, index);
		}
		
		[Test]
		public static void WorldToLocalPosition()
		{
			//Valid
			Assert.AreEqual(new int2(0, 0), Chunk.WorldToLocalPosition(0, 0));
			Assert.AreEqual(new int2(0, 0), Chunk.WorldToLocalPosition(Chunk.Width, 0));
			Assert.AreEqual(new int2(0, 0), Chunk.WorldToLocalPosition(0, Chunk.Length));
			Assert.AreEqual(new int2(0, 0), Chunk.WorldToLocalPosition(Chunk.Width, Chunk.Length));

			Assert.AreEqual(new int2(1, 0), Chunk.WorldToLocalPosition(Chunk.Width + 1, 0));
			Assert.AreEqual(new int2(0, 1), Chunk.WorldToLocalPosition(0, Chunk.Length + 1));
			Assert.AreEqual(new int2(1, 1), Chunk.WorldToLocalPosition(Chunk.Width + 1, Chunk.Length + 1));
		}
		
		[Test]
		public static void LocalToWorldPosition()
		{
			//Valid
			Assert.AreEqual(new int2(0, 0), Chunk.LocalToWorldPosition(0, 0));

			if(Chunk.Width >= 1)
			{
				Assert.AreEqual(new int2(1 * Chunk.Width, 0), Chunk.LocalToWorldPosition(1, 0));
				Assert.AreEqual(new int2(0, 1 * Chunk.Length), Chunk.LocalToWorldPosition(0, 1));
				Assert.AreEqual(new int2(1 * Chunk.Width, 1 * Chunk.Length), Chunk.LocalToWorldPosition(1, 1));
			}
		}

		#region Validation

		[Test]
		public static void IsValidPosition()
		{
			Assert.IsTrue(Chunk.IsValidPosition(0, 0, 0));
			
			Assert.IsTrue(Chunk.IsValidPosition(Chunk.Width - 1, 0, 0));
			Assert.IsTrue(Chunk.IsValidPosition(0, Chunk.Height - 1, 0));
			Assert.IsTrue(Chunk.IsValidPosition(0, 0, Chunk.Width - 1));

			Assert.IsFalse(Chunk.IsValidPosition(Chunk.Width, 0, 0));
			Assert.IsFalse(Chunk.IsValidPosition(0, Chunk.Height, 0));
			Assert.IsFalse(Chunk.IsValidPosition(0, 0, Chunk.Width));
		}

		[Test]
		public static void IsValidBlockIndex()
		{
			Assert.IsTrue(Chunk.IsValidBlockIndex(0));
			Assert.IsTrue(Chunk.IsValidBlockIndex(Chunk.TotalBlockCount - 1));

			Assert.IsFalse(Chunk.IsValidBlockIndex(-1));
			Assert.IsFalse(Chunk.IsValidBlockIndex(Chunk.TotalBlockCount));
		}

		#endregion
		
		[Test]
		public static void CheckAdjacentBlockIsSolid()
		{
			Chunk chunk = new(0, 0, Allocator.Persistent);
			
			//Fill chunk with stone blocks
			for(int i = 0; i < chunk.blocks.Length; i++)
			{
				chunk.blocks[i] = new Block
				{
					blockType = BlockType.Stone
				};
			}

			//First block
			{
				int3 blockPosition = new(0, 0, 0);
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Forward, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Right, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Up, blockPosition));

				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Left, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Backward, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Down, blockPosition));
			}
			
			//Center block
			{
				int3 blockPosition = new(Chunk.Width / 2, Chunk.Height / 2, Chunk.Length / 2);
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Forward, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Backward, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Left, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Right, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Up, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Down, blockPosition));
			}
			
			//Last block
			{
				int3 blockPosition = new(Chunk.Width - 1, Chunk.Height - 1, Chunk.Length - 1);
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Left, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Backward, blockPosition));
				Assert.IsTrue(chunk.CheckAdjacentBlockIsSolid(Direction.Down, blockPosition));
				
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Forward, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Right, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Up, blockPosition));
			}
			
			//Out of bounds block
			{
				int3 blockPosition = new(Chunk.Width + 10, Chunk.Height + 10, Chunk.Length + 10);
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Forward, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Backward, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Left, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Right, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Up, blockPosition));
				Assert.IsFalse(chunk.CheckAdjacentBlockIsSolid(Direction.Down, blockPosition));
			}

			chunk.Dispose();
		}
	}
}
