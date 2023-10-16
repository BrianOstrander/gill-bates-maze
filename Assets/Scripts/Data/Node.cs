using System;
using System.Collections.Generic;
using UnityEngine;

namespace GillBates.Data
{
    public class Node
    {
        Maze maze;
        
        public Vector2Int Position { get; private set; }
        public bool IsSolid { get; private set; }

        int cheesePower;
        
        /// <summary>
        /// The higher the cheese power, the closer we are to the cheese!
        /// </summary>
        public int CheesePower
        {
            get => cheesePower;
            set
            {
                if (cheesePower == value)
                {
                    return;
                }

                cheesePower = value;
                CheesePowerUpdate?.Invoke(value);
            }
        }

        /// <summary>
        /// Cheese power spreads from the cheese, everytime a node's cheese power is updated this is called.
        /// </summary>
        public event Action<int> CheesePowerUpdate;
        
        public Node(
            Maze maze,
            Vector2Int position,
            bool isSolid
        )
        {
            this.maze = maze;
            
            Position = position;
            IsSolid = isSolid;
        }

        public void Tick()
        {
            if (IsSolid)
            {
                return;
            }

            var maximumNeighborCheesePower = GetMaximumNeighborCheesePower(
                Directions.Up,
                Directions.Right,
                Directions.Down,
                Directions.Left
            );
            
            // Cheese power falls off over distances, so either we are the highest cheese power of our neighbors, or 
            // one of our neighbors has a higher cheese power than us.
            
            CheesePower = Mathf.Max(
                CheesePower,
                maximumNeighborCheesePower - 1
            );
        }
        
        /// <summary>
        /// From the directions provided, get the neighbor with the highest cheese power.
        /// </summary>
        /// <remarks>
        /// The higher the cheese power, the closer we are to the cheese!
        /// </remarks>
        /// <param name="directions"></param>
        /// <returns>The highest cheese power from neighbors in the provided directions</returns>
        int GetMaximumNeighborCheesePower(params Directions[] directions)
        {
            var result = 0;
            
            for (var i = 0; i < directions.Length; i++)
            {
                if (TryGetNeighbor(directions[i], out var neighbor))
                {
                    result = Mathf.Max(
                        result,
                        neighbor.CheesePower
                    );
                }
            }

            return result;
        }
        
        /// <summary>
        /// Try to get a non-solid neighbor to the direction provided.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="neighbor"></param>
        /// <returns>True if a non-solid neighbor exists in the direction provided, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool TryGetNeighbor(
            Directions direction,
            out Node neighbor
        )
        {
            neighbor = null;
            
            var positionDelta = Vector2Int.zero;
            
            switch (direction)
            {
                case Directions.Up:
                {
                    positionDelta = Vector2Int.up;
                    break;
                }
                case Directions.Right:
                {
                    positionDelta = Vector2Int.right;
                    break;
                }
                case Directions.Down:
                {
                    positionDelta = Vector2Int.down;
                    break;
                }
                case Directions.Left:
                {
                    positionDelta = Vector2Int.left;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            var neighborPosition = Position + positionDelta;

            if (neighborPosition.x < 0)
            {
                return false;
            }
            
            if (neighborPosition.y < 0)
            {
                return false;
            }
            
            if (maze.Size.y <= neighborPosition.y)
            {
                return false;
            }

            var row = maze.Nodes[neighborPosition.y];
            
            if (row.Count <= neighborPosition.x)
            {
                return false;
            }

            neighbor = row[neighborPosition.x];
            return !IsSolid && !neighbor.IsSolid;
        }
    }
}