using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24 {

	/// <summary>
	/// Functions related to tile-space math.
	/// </summary>
	public static class TileSpace {

		public static readonly Vector2Int[] Deltas = {
			new Vector2Int(0, 1),
			new Vector2Int(1, 1),
			new Vector2Int(1, 0),
			new Vector2Int(1, -1),
			new Vector2Int(0, -1),
			new Vector2Int(-1, -1),
			new Vector2Int(-1, 0),
			new Vector2Int(-1, 1)
		};

		public static IEnumerable<Vector2Int> CardinalNeighborsOf(Vector2Int tile) {
			yield return tile + Vector2Int.up;
			yield return tile + Vector2Int.right;
			yield return tile + Vector2Int.down;
			yield return tile + Vector2Int.left;
		}

		public static IEnumerable<Vector2Int> DiagonalNeighborsOf(Vector2Int tile) {
			yield return tile + new Vector2Int(1, 1);
			yield return tile + new Vector2Int(1, -1);
			yield return tile + new Vector2Int(-1, -1);
			yield return tile + new Vector2Int(-1, 1);
		}

		public static IEnumerable<Vector2Int> AllNeighborsOf(Vector2Int tile) =>
			CardinalNeighborsOf(tile).Concat(DiagonalNeighborsOf(tile));

		public static RectInt MakeCenteredBounds(Vector2Int center, int width, int height) {
			var minX = center.x - Mathf.FloorToInt((width - 1) / 2f);
			var minY = center.y - Mathf.FloorToInt((height - 1) / 2f);

			return new RectInt(minX, minY, width, height);
		}

		public static IEnumerable<Vector2Int> TilesInBounds(RectInt bounds) {
			for (var x = bounds.xMin; x < bounds.xMax; x++)
			for (var y = bounds.yMin; y < bounds.yMax; y++)
				yield return new Vector2Int(x, y);
		}

		public static RectInt Grow(RectInt bounds, int border) {
			return new RectInt(
				bounds.xMin - border,
				bounds.yMin - border,
				bounds.width + border * 2,
				bounds.height + border * 2
			);
		}

		public static Vector2Int GetDestinationTile(Vector2Int origin, GridDirection direction) {

			switch (direction) {

				case GridDirection.XMinus:
					return origin + Deltas[6];
				case GridDirection.XPlus:
					return origin + Deltas[2];
				case GridDirection.ZPlus:
					return origin + Deltas[0];
				case GridDirection.ZMinus:
					return origin + Deltas[4];
				default:
					return origin;
			}

		}

	}

}