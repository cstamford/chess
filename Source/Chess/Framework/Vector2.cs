using System;

namespace Chess.Framework
{
    /// <summary>
    /// Represents a 2 dimensional vector, coordinate, or point in space.
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// The x coordinate of this vector.
        /// </summary>
        public int X { get { return _x; } }

        /// <summary>
        /// The y coordinate of this vector.
        /// </summary>
        public int Y { get { return _y; } }

        /// <summary>
        /// Constructs a vector in the given location.
        /// </summary>
        /// <param name="x">The provided x coordinate.</param>
        /// <param name="y">The provided y coordinate.</param>
        public Vector2(int x, int y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Compares this vector to a second vector.
        /// </summary>
        /// <param name="other">The second vector.</param>
        /// <returns>True if both elements are equal, false otherwise.</returns>
        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// The x coordinate of this vector.
        /// </summary>
        private int _x;

        /// <summary>
        /// The y coordinate of this vector.
        /// </summary>
        private int _y;
    }
}