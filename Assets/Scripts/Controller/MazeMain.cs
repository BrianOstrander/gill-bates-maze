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
        int cheesePowerMax;
        
        [SerializeField]
        GameObject wallPrefab;
        
        [SerializeField]
        PathBehaviour pathPrefab;
        
        [SerializeField]
        GameObject cheesePrefab;
        
        [SerializeField]
        MouseBehaviour mousePrefab;

        [SerializeField]
        Transform cameraRoot;

        [SerializeField]
        float tickDelay;
        
        Maze maze;
        Vector2Int beginPosition;
        Vector2Int endPosition;
        float tickDelayRemaining;

        MouseController mouse;
        
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
                maze.Size.x - 1,
                maze.Size.y - 2
            );
            
            cameraRoot.position = new Vector3(
                endPosition.x / 2f,
                endPosition.y / 2f,
                cameraRoot.position.z
            );
            
            for (var y = 0; y < maze.Size.y; y++)
            {
                for (var x = 0; x < maze.Size.x; x++)
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

            var cheeseView = Instantiate(
                cheesePrefab,
                new Vector3(
                    endPosition.x,
                    maze.Size.y - endPosition.y
                ),
                Quaternion.identity
            );
            
            var mouseView = Instantiate(
                mousePrefab
            );

            mouse = new MouseController();
            mouse.Initialize(
                maze,
                beginPosition,
                mouseView
            );
        }

        void InstantiateWall(
            Node node
        )
        {
            Instantiate(
                wallPrefab,
                new Vector3(
                    node.Position.x,
                    maze.Size.y - node.Position.y
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
                    maze.Size.y - node.Position.y,
                    1
                ),
                Quaternion.identity
            );

            node.CheesePower = node.Position == endPosition ? cheesePowerMax : 0;
            
            instance.Initialize(
                node,
                cheesePowerMax
            );
        }

        void Update()
        {
            tickDelayRemaining -= Time.deltaTime;

            if (0f < tickDelayRemaining)
            {
                return;
            }

            tickDelayRemaining = tickDelay;
            Tick();
        }

        void Tick()
        {
            for (var y = 0; y < maze.Size.y; y++)
            {
                for (var x = 0; x < maze.Size.x; x++)
                {
                    maze.Nodes[y][x].Tick();
                }
            }
            
            mouse.Tick();
        }
    }
}