using System;
using GillBates.Data;
using GillBates.View;
using UnityEngine;

namespace GillBates.Controller
{
    public class MazeMain : MonoBehaviour
    {

        [SerializeField, TextArea]
        string textInput;

        [SerializeField]
        GameObject wallPrefab;
        
        [SerializeField]
        PathBehaviour pathPrefab;

        Maze maze;
        Vector2Int beginPosition;
        Vector2Int endPosition;
        
        void Awake()
        {
            maze = new Maze();
            maze.Load(textInput);
            
            // Since I'm treating this problem as a solution prototype, I'm simplifying the problem by making
            // walls into solid cells, thus the begin and end positions would be calculated as follows:

            beginPosition = new Vector2Int(
                0,
                1
            );

            endPosition = new Vector2Int(
                maze.Nodes[0].Count - 1,
                maze.Nodes.Count - 2
            );
            
            for (var y = 0; y < maze.Nodes.Count; y++)
            {
                for (var x = 0; x < maze.Nodes[y].Count; x++)
                {
                    var current = maze.Nodes[y][x];

                    if (current.IsSolid)
                    {
                        InstantiateWall(current);
                    }
                    else
                    {
                        InstantiatePath(current);
                    }
                }
            }
        }

        void InstantiateWall(
            Node node
        )
        {
            Instantiate(
                wallPrefab,
                new Vector3(
                    node.Position.x,
                    maze.Nodes.Count - node.Position.y
                ),
                Quaternion.identity
            );
        }
        
        void InstantiatePath(
            Node node
        )
        {
            var instance = Instantiate(
                pathPrefab,
                new Vector3(
                    node.Position.x,
                    maze.Nodes.Count - node.Position.y,
                    1
                ),
                Quaternion.identity
            );
        }
    }
}