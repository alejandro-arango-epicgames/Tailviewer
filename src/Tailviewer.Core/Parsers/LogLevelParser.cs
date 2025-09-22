using System;
using System.Diagnostics.Contracts;
using Tailviewer.Api;

// ReSharper disable once CheckNamespace
namespace Tailviewer.Core
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LogLevelParser
	{
		/// <summary>
		///     Parses the given line and returns the left most log level
		///     from it.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		[Pure]
		public LevelFlags DetermineLevelFromLine(string line)
		{
			DetermineLevelsFromLine(line, out var leftMost);
			return leftMost;
		}

		/// <summary>
		///     Parses the given line and extracts Unreal Engine log levels from it,
		///     if there are any.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="leftMost">The left-most log level in the given <paramref name="line"/></param>
		/// <returns>The last log level in the given <paramref name="line"/></returns>
		public LevelFlags DetermineLevelsFromLine(string line, out LevelFlags leftMost)
		{
			LevelFlags rightMost = LevelFlags.None;
			leftMost = LevelFlags.None;
			int index = int.MaxValue;

			if (line == null)
			{
				leftMost = LevelFlags.Other;
				return LevelFlags.Other;
			}

			var comparison = StringComparison.InvariantCultureIgnoreCase; // Unreal logs can have different cases

			// Unreal Engine log format: "LogCategory: Level: Message"
			// Look for the specific pattern ": Level:" to ensure we're matching the actual log level

			// Fatal level
			var idx = line.IndexOf(": Fatal:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Fatal;
				if (idx < index)
				{
					leftMost = LevelFlags.Fatal;
					index = idx;
				}
			}

			// Error level
			idx = line.IndexOf(": Error:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Error;
				if (idx < index)
				{
					leftMost = LevelFlags.Error;
					index = idx;
				}
			}

			// Warning level
			idx = line.IndexOf(": Warning:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Warning;
				if (idx < index)
				{
					leftMost = LevelFlags.Warning;
					index = idx;
				}
			}

			// Info level equivalents: Display and Log
			idx = line.IndexOf(": Display:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Info;
				if (idx < index)
				{
					leftMost = LevelFlags.Info;
					index = idx;
				}
			}

			idx = line.IndexOf(": Log:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Info;
				if (idx < index)
				{
					leftMost = LevelFlags.Info;
					index = idx;
				}
			}

			// Debug level equivalents: Verbose and VeryVerbose
			idx = line.IndexOf(": VeryVerbose:", comparison); // Check VeryVerbose first (longer string)
			if (idx != -1)
			{
				rightMost |= LevelFlags.Debug;
				if (idx < index)
				{
					leftMost = LevelFlags.Debug;
					index = idx;
				}
			}
			else
			{
				idx = line.IndexOf(": Verbose:", comparison);
				if (idx != -1)
				{
					rightMost |= LevelFlags.Debug;
					if (idx < index)
					{
						leftMost = LevelFlags.Debug;
						index = idx;
					}
				}
			}

			// Trace level - keep as fallback for very detailed logs
			idx = line.IndexOf(": Trace:", comparison);
			if (idx != -1)
			{
				rightMost |= LevelFlags.Trace;
				if (idx < index)
				{
					leftMost = LevelFlags.Trace;
					index = idx;
				}
			}

			if (leftMost == LevelFlags.None)
				leftMost = LevelFlags.Other;

			if (rightMost == LevelFlags.None)
				rightMost = LevelFlags.Other;
			return rightMost;
		}
	}
}
