using System;
using GillBates.Data;
using GillBates.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GillBates.Controller
{
    public class MainController : MonoBehaviour
    {
        
        /// <summary>
        /// The default maze.
        /// </summary>
        [SerializeField, TextArea]
        string defaultInput;

        /// <summary>
        /// The maximum distance the "smell" of the cheese will permeate the maze. If it is too small then
        /// our poor mouse won't find the cheese!
        /// </summary>
        [SerializeField]
        int cheesePowerDefault;
        
        [SerializeField]
        GameObject wallPrefab;
        
        [SerializeField]
        PathBehaviour pathPrefab;
        
        [SerializeField]
        GameObject cheesePrefab;
        
        [SerializeField]
        MouseBehaviour mousePrefab;

        [SerializeField]
        Camera mazeCamera;

        [SerializeField]
        GameObject controlPanel;

        [SerializeField]
        PopupBehaviour popup;
        
        [SerializeField]
        int restartSceneIndex;

        [SerializeField]
        Slider cheesePowerSlider;

        [SerializeField]
        Text cheesePowerValueLabel;
        
        /// <summary>
        /// How long to weight between ticks of the simulation. Setting this to zero will update it every frame.
        /// </summary>
        [SerializeField]
        float tickDelay;
        
        Maze maze;
        Vector2Int beginPosition;
        Vector2Int endPosition;
        bool isDefaultInput;
        float tickDelayRemaining;
        int lastCheesePower;
        int? framesUntilCheesePowerUpdate;

        MouseController mouse;

        void Awake()
        {
            controlPanel.SetActive(false);
            popup.Close();
            
            var input = defaultInput;
            isDefaultInput = true;

            if (PersistantData.IsFirstLoad.Value)
            {
                // This is the first time we're loading. So lets reset everything in case we previously crashed
                // or the user force quit at a weird time.
                PersistantData.IsLoadingFromCache.Value = false;
                PersistantData.CheesePower.Value = cheesePowerDefault;
                
                PersistantData.IsFirstLoad.Value = false;
            }
            else if (PersistantData.IsLoadingFromCache.Value)
            {
                // We have been restarted and are supposed to parse a maze.
                
                input = PersistantData.CachedMaze.Value;
                PersistantData.IsLoadingFromCache.Value = false;
                isDefaultInput = false;
            }

            lastCheesePower = PersistantData.CheesePower.Value;
            cheesePowerValueLabel.text = PersistantData.CheesePower.Value.ToString("N0");
            cheesePowerSlider.value = PersistantData.CheesePower.Value;

            try
            {
                Initialize(input);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                OnError();
                return;
            }
            
            controlPanel.SetActive(true);
        }

        void Initialize(string input)
        {
            maze = new Maze();
            maze.Load(input);
            
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
            
            mazeCamera.transform.position = new Vector3(
                endPosition.x / 2f,
                endPosition.y / 2f,
                mazeCamera.transform.position.z
            );

            mazeCamera.orthographicSize = 0.5f * Mathf.Max(
                maze.Size.x,
                maze.Size.y
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

            Instantiate(
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
                endPosition,
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

            node.CheesePower = node.Position == endPosition ? PersistantData.CheesePower.Value : 0;
            
            instance.Initialize(node);
        }

        void Update()
        {
            if (framesUntilCheesePowerUpdate.HasValue)
            {
                framesUntilCheesePowerUpdate--;

                if (framesUntilCheesePowerUpdate == 0)
                {
                    framesUntilCheesePowerUpdate = null;
                    ResetCheesePower();
                }
            }
            
            tickDelayRemaining -= Time.deltaTime;

            if (0f < tickDelayRemaining)
            {
                return;
            }

            tickDelayRemaining = tickDelay;

            try
            {
                Tick();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                OnError();
            }
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

        /// <summary>
        /// Go through all non-solid nodes and set their cheese power to zero if not the end position, or to the
        /// cheese power if they are the end position.
        /// </summary>
        void ResetCheesePower()
        {
            for (var y = 0; y < maze.Size.y; y++)
            {
                for (var x = 0; x < maze.Size.x; x++)
                {
                    if (maze.TryGetNode(new Vector2Int(x, y), out var node))
                    {
                        node.CheesePower = node.Position == endPosition ? PersistantData.CheesePower.Value : 0;
                    }
                }
            }
        }
        
        public void OnClickRestart()
        {
            PersistantData.IsLoadingFromCache.Value = false;
            OnRestart();
        }
        
        public void OnClickPasteAndRestart()
        {
            PersistantData.IsLoadingFromCache.Value = true;
            PersistantData.CachedMaze.Value = GUIUtility.systemCopyBuffer;
            OnRestart();
        }

        void OnRestart()
        {
            controlPanel.SetActive(false);
            SceneManager.LoadScene(restartSceneIndex);
        }

        public void OnClickCheesePowerReset()
        {
            OnUpdateCheesePower(cheesePowerDefault);
        }

        public void OnUpdateCheesePower(float floatValue)
        {
            var value = Mathf.FloorToInt(floatValue);

            if (value == lastCheesePower)
            {
                return;
            }

            lastCheesePower = value;
            PersistantData.CheesePower.Value = value;
            cheesePowerValueLabel.text = value.ToString("N0");
            
            framesUntilCheesePowerUpdate = 15;
        }

        void OnError()
        {
            enabled = false;
            controlPanel.SetActive(false);
            
            string title;
            string description;

            if (isDefaultInput)
            {
                title = "Oops!";
                description = "We made a mistake, lets try this again!";
            }
            else
            {
                title = "Oops! Invalid Maze";
                description = "That maze was a real head scratcher... Try copying a different one to your clipboard next time! Can I show you one that works?";
            }
                
            popup.Open(
                title,
                description,
                OnClickRestart
            );
        }
    }
}