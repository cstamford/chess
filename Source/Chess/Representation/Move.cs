using Chess.Framework;
using System;

namespace Chess.Representation
{
    /// <summary>
    /// A move decribes two points in the game world.
    /// </summary>
    public struct Move : IEquatable<Move>
    {
        /// <summary>
        /// Gets the coordinates of the origin point of this move.
        /// </summary>
        public Vector2 From { get { return _from; } }

        /// <summary>
        /// Gets the coordinates of the end point of this move.
        /// </summary>
        public Vector2 To { get { return _to; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Move(Vector2 from, Vector2 to)
        {
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Compares this move to a second move.
        /// </summary>
        /// <param name="other">The second move.</param>
        /// <returns>True if both vectors are equal, false otherwise.</returns>
        public bool Equals(Move other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        /// <summary>
        /// The coordinates of the end point of this move.
        /// </summary>
        private Vector2 _from;

        /// <summary>
        /// The coordinates of the end point of this move.
        /// </summary>
        private Vector2 _to;
    }
}
