using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ConestogaDefence.Maps;

namespace ConestogaDefence
{
    public class SearchNode
    {
        public SearchNode[] Neighbors { get; set; }
        public SearchNode Parent { get; set; }
        public Point Position { get; set; }
        public float DistanceToGoal { get; set; }
        public float DistanceTraveled { get; set; }
        public bool Walkable { get; set; }
        public bool InOpenList { get; set; }
        public bool InClosedList { get; set; }
    }

    public class PathFinder
    {
        private static readonly int msCellWidth = 64;
        private static readonly int msCellHeight = 64;

        private List<SearchNode> mOpenList = new List<SearchNode>();
        private List<SearchNode> mClosedList = new List<SearchNode>();
        private SearchNode[,] mSearchNodes;
        private int mLevelWidth;
        private int mLevelHeight;

        public PathFinder(in Map map)
        {
            mLevelWidth = map.NumOfColumn;
            mLevelHeight = map.NumOfRow;

            initializeSearchNodes(map);
        }

        private float findDistance(in Point point1, in Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        private void initializeSearchNodes(in Map map)
        {
            mSearchNodes = new SearchNode[mLevelWidth, mLevelHeight];

            SearchNode node;
            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    node = new SearchNode
                    {
                        Position = new Point(x, y),
                        Walkable = map.Layout[y, x] == -2
                    };

                    if (node.Walkable == true)
                    {
                        node.Neighbors = new SearchNode[4];
                        mSearchNodes[x, y] = node;
                    }
                }
            }

            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    node = mSearchNodes[x, y];

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y),
                    };

                    Point position;
                    SearchNode neighbor;
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        position = neighbors[i];

                        if (position.X < 0 || position.X > mLevelWidth - 1 ||
                            position.Y < 0 || position.Y > mLevelHeight - 1)
                        {
                            continue;
                        }

                        neighbor = mSearchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }

                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }

        private void resetSearchNodes()
        {
            mOpenList.Clear();
            mClosedList.Clear();

            SearchNode node;
            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    node = mSearchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        private List<Vector2> findFinalPath(in SearchNode startNode,
            in SearchNode endNode)
        {
            mClosedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;
            while (parentTile != startNode)
            {
                mClosedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            mClosedList.Add(parentTile);

            List<Vector2> finalPath = new List<Vector2>();
            for (int i = mClosedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(mClosedList[i].Position.X * msCellWidth,
                    mClosedList[i].Position.Y * msCellHeight));
            }

            return finalPath;
        }
        
        private SearchNode findBestNode()
        {
            SearchNode currentTile = mOpenList[0];
            float smallestDistanceToGoal = float.MaxValue;

            for (int i = 0; i < mOpenList.Count; i++)
            {
                if (mOpenList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = mOpenList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }
        
        public List<Vector2> FindPath(in Point startPoint, in Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            resetSearchNodes();

            SearchNode startNode = mSearchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = mSearchNodes[endPoint.X, endPoint.Y];

            startNode.InOpenList = true;

            startNode.DistanceToGoal = findDistance(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            mOpenList.Add(startNode);

            SearchNode currentNode;
            while (mOpenList.Count > 0)
            {
                currentNode = findBestNode();

                if (currentNode == null)
                {
                    break;
                }
                
                if (currentNode == endNode)
                {
                    return findFinalPath(startNode, endNode);
                }

                SearchNode neighbor;
                float distanceTraveled;
                float heuristic;
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    neighbor = currentNode.Neighbors[i];

                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    distanceTraveled = currentNode.DistanceTraveled + 1;

                    heuristic = findDistance(neighbor.Position, endPoint);

                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        neighbor.DistanceTraveled = distanceTraveled;
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        neighbor.Parent = currentNode;
                        neighbor.InOpenList = true;
                        mOpenList.Add(neighbor);
                    }
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                mOpenList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            return new List<Vector2>();
        }
    }
}