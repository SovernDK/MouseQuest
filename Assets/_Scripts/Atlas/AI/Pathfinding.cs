using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Atlas.AI.Grid
{
    public class Pathfinding
    {
        public Vector3Int[] _neighbours = new Vector3Int[]
        {
            Vector3Int.forward,
            Vector3Int.back,
            Vector3Int.left,
            Vector3Int.right,
        };

        public Dictionary<Vector3Int, Node> BreadthFirst(Dictionary<Vector3Int, Node> nodes, Vector3Int start, Vector3Int end)
        {
            Queue<Node> frontier = new Queue<Node>();
            frontier.Enqueue(nodes[start]);
            Dictionary<Vector3Int, Node> cameFrom = new Dictionary<Vector3Int, Node>();
            cameFrom[start] = nodes[start];

            while(frontier.Count > 0)
            {
                Node current = frontier.Dequeue();

                if(current == nodes[end]) break;

                foreach(Vector3Int next in GetNeighbours(nodes, current.Coordinates))
                {
                    if(!cameFrom.ContainsKey(next) && !nodes[next].IsOccupied)
                    {
                        frontier.Enqueue(nodes[next]);
                        cameFrom[next] = current;
                    }
                }
            }   

            return cameFrom;
        }

        public Vector3Int[] GetNeighbours(Dictionary<Vector3Int, Node> nodes, Vector3Int position)
        {
            List<Vector3Int> result = new List<Vector3Int>();
            foreach(Vector3Int direction in _neighbours)
            {
                if(nodes.TryGetValue(position + direction, out Node node))
                {
                    bool isBlocked = NavMesh.Raycast(nodes[position].Position, node.Position, out NavMeshHit hit, NavMesh.AllAreas);
                    if(!isBlocked)
                        result.Add(node.Coordinates);
                }
            }

            return result.ToArray();
        }

        public List<Node> GetRadius(Dictionary<Vector3Int, Node> nodes, Vector3Int center, float distance, bool withCenter = false)
        {
            Queue<Node> frontier = new Queue<Node>();
            frontier.Enqueue(nodes[center]);
            List<Node> within = new List<Node>();

            while(frontier.Count > 0)
            {
                Node current = frontier.Dequeue();

                if(Vector3Int.Distance(center, current.Coordinates) >= distance) break;
                foreach(Vector3Int next in GetNeighbours(nodes, current.Coordinates))
                {
                    if(!within.Contains(nodes[next])) 
                    {
                        frontier.Enqueue(nodes[next]);
                        within.Add(nodes[next]);
                    }
                }
            }

            if(!withCenter)
                within.Remove(nodes[center]);

            return within;
        }

        public Queue<Node> CreatePath(Dictionary<Vector3Int, Node> nodes, Vector3Int start, Vector3Int goal)
        {
            Dictionary<Vector3Int, Node> cameFrom = BreadthFirst(nodes, start, goal);

            Node node = nodes[goal];
            Queue<Node> path = new Queue<Node>();

            while(node != nodes[start])
            {
                if (!cameFrom.ContainsKey(node.Coordinates)) break;
                path.Enqueue(node);
                node = cameFrom[node.Coordinates];
            }
            path.Enqueue(nodes[start]);
            
            return new Queue<Node>(path.ToArray().Reverse().ToArray());
        }
    }
}