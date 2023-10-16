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
            
            if (maze.Nodes.Count <= neighborPosition.y)
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