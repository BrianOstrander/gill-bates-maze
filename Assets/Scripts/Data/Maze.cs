using System.Collections.Generic;
using UnityEngine;

namespace GillBates.Data
{
    public class Maze
    {
        public List<List<Node>> Nodes = new();
        
        public Vector2Int Size { get; private set; }
        
        public void Load(string text)
        {
            var position = Vector2Int.zero;
            Nodes.Add(new List<Node>());
            
            for (var i = 0; i < text.Length; i++)
            {
                var isSolid = false;
                
                switch (text[i])
                {
                    case '+':
                    case '-':
                    case '|':
                    {
                        isSolid = true;
                        break;
                    }
                    case ' ':
                    {
                        break;
                    }
                    case '\n':
                    {
                        position = new Vector2Int(
                            0,
                            position.y + 1
                        );
                        Nodes.Add(new List<Node>());
                        continue;
                    }
                }
                
                Nodes[position.y].Add(
                    new Node(
                        this,
                        position,
                        isSolid
                    )
                );
                
                position.x++;
            }

            Size = new Vector2Int(
                Nodes[0].Count,
                Nodes.Count
            );
        }
        
        public bool TryGetNode(
            Vector2Int position,
            out Node node
        )
        {
            node = null;
            
            if (position.x < 0)
            {
                return false;
            }

            if (position.y < 0)
            {
                return false;
            }

            if (Size.y <= position.y)
            {
                return false;
            }

            if (Size.x <= position.x)
            {
                return false;
            }

            node = Nodes[position.y][position.x];
            return !node.IsSolid;
        }
    }
}