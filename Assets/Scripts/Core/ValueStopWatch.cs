namespace VoxelEngine.Core
{
	using System;
	using System.Diagnostics;

	public struct ValueStopWatch
	{
		private long startTimestamp;
		private long elapsedTimestamp;
		private bool isRunning;

		private ValueStopWatch(long startTimestamp)
		{
			this.startTimestamp = startTimestamp;
			this.elapsedTimestamp = 0;
			this.isRunning = true;
		}

		public static ValueStopWatch StartNew()
		{
			return new ValueStopWatch(Stopwatch.GetTimestamp());
		}

		public TimeSpan Elapsed
		{
			get
			{
				var endTimestamp = isRunning ? Stopwatch.GetTimestamp() : startTimestamp;
				var timestampDelta = endTimestamp - startTimestamp + elapsedTimestamp;
				return TimeSpan.FromTicks(timestampDelta * (TimeSpan.TicksPerSecond / Stopwatch.Frequency));
			}
		}

		public long ElapsedMilliseconds => Elapsed.Ticks / TimeSpan.TicksPerMillisecond;

		public long ElapsedTicks => Elapsed.Ticks;

		public static ValueStopWatch Start()
		{
			return StartNew();
		}
		
		public void Stop()
		{
			if (isRunning)
			{
				elapsedTimestamp += Stopwatch.GetTimestamp() - startTimestamp;
				isRunning = false;
			}
		}

		public void Restart()
		{
			startTimestamp = Stopwatch.GetTimestamp();
			elapsedTimestamp = 0;
			isRunning = true;
		}

		public void Reset()
		{
			elapsedTimestamp = 0;
			startTimestamp = 0;
			isRunning = false;
		}
	}
}
