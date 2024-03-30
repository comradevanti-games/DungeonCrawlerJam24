using System;
using UnityEngine;

namespace DGJ24.TileSpace
{
    /// <summary>
    /// Represents the transform of an object that lives in tile-space.
    /// </summary>
    public interface ITileTransform
    {
        /// <summary>
        /// The objects position.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// The direction the object is facing.
        /// </summary>
        public CardinalDirection Forward { get; set; }

        public CardinalDirection LocalToGlobal(CardinalDirection local)
        {
            return Forward switch
            {
                CardinalDirection.Right
                    => local switch
                    {
                        CardinalDirection.Forward => CardinalDirection.Right,
                        CardinalDirection.Backward => CardinalDirection.Left,
                        CardinalDirection.Left => CardinalDirection.Forward,
                        CardinalDirection.Right => CardinalDirection.Backward,
                        _ => throw new ArgumentOutOfRangeException(nameof(local), local, null)
                    },
                CardinalDirection.Left
                    => local switch
                    {
                        CardinalDirection.Forward => CardinalDirection.Left,
                        CardinalDirection.Backward => CardinalDirection.Right,
                        CardinalDirection.Left => CardinalDirection.Backward,
                        CardinalDirection.Right => CardinalDirection.Forward,
                        _ => throw new ArgumentOutOfRangeException(nameof(local), local, null)
                    },
                CardinalDirection.Forward
                    => local switch
                    {
                        CardinalDirection.Forward => CardinalDirection.Forward,
                        CardinalDirection.Backward => CardinalDirection.Backward,
                        CardinalDirection.Left => CardinalDirection.Left,
                        CardinalDirection.Right => CardinalDirection.Right,
                        _ => throw new ArgumentOutOfRangeException(nameof(local), local, null)
                    },
                CardinalDirection.Backward
                    => local switch
                    {
                        CardinalDirection.Forward => CardinalDirection.Backward,
                        CardinalDirection.Backward => CardinalDirection.Forward,
                        CardinalDirection.Left => CardinalDirection.Right,
                        CardinalDirection.Right => CardinalDirection.Left,
                        _ => throw new ArgumentOutOfRangeException(nameof(local), local, null)
                    },
                _ => throw new ArgumentOutOfRangeException(nameof(Forward), Forward, null)
            };
        }

        public void Rotate(RotationDirection direction)
        {
            Forward = TileSpaceMath.RotateDirection(Forward, direction);
        }

        public void MoveIn(CardinalDirection direction)
        {
            var nextTile = TileSpaceMath.MoveByDirection(Position, direction);
            Position = nextTile;
        }
    }
}
