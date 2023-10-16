using System;
using GillBates.Data;
using GillBates.View;
using UnityEngine;

namespace GillBates.Controller
{
    public class MouseController
    {
        Maze maze;
        Vector2Int position;
        MouseBehaviour view;
        
        public void Initialize(
            Maze maze,
            Vector2Int beginPosition,
            MouseBehaviour view
        )
        {
            this.maze = maze;
            position = beginPosition;
            this.view = view;
            
            var beginPositionWorld = new Vector3(
                beginPosition.x,
                maze.Size.y - beginPosition.y
            );
            
            view.Move(
                beginPositionWorld,
                true
            );
        }

        public void Tick()
        {
            if (view.IsMoving)
            {
                return;
            }

            var isBetterNeighborFound = TryGetBestNeighbor(
                out var neighbor,
                Directions.Up,
                Directions.Right,
                Directions.Down,
                Directions.Left
            );

            if (!isBetterNeighborFound)
            {
                return;
            }

            position = neighbor.Position;

            var movePosition = new Vector3(
                position.x,
                maze.Size.y - position.y
            );
            
            view.Move(
                movePosition
            );
        }

        /// <summary>
        /// From the provided directions, try to find the first neighbor with a higher cheese power than our current
        /// node.
        /// </summary>
        /// <param name="neighbor"></param>
        /// <param name="directions"></param>
        /// <returns>True if a neighbor with a higher cheese power is found, otherwise false.</returns>
        /// <exception cref="Exception"></exception>
        bool TryGetBestNeighbor(
            out Node neighbor,
            params Directions[] directions
        )
        {
            if (!maze.TryGetNode(position, out var currentNode))
            {
                throw new Exception($"Mouse is out of bounds or in a solid node at position {position}");
            }

            var currentCheesePower = currentNode.CheesePower;
            
            for (var i = 0; i < directions.Length; i++)
            {
                if (currentNode.TryGetNeighbor(directions[i], out neighbor))
                {
                    if (currentCheesePower < neighbor.CheesePower)
                    {
                        return true;
                    }
                }
            }

            neighbor = null;
            return false;
        }
    }
}