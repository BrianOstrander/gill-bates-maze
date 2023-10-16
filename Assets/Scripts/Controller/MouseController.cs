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

            var movePosition = new Vector3(
                neighbor.Position.x,
                neighbor.Position.y
            );
        }

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