using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapSaver
    {
        public List<List<Node>> ConvertToNode(Tree tree)
        {
            int x = 0;
            List<Tree> branches = new List<Tree>(){tree};
            List<List<Node>> result = new List<List<Node>>();
            while (branches.Count>0)
            {
                List<Node> temp = new List<Node>(); 
                for (var y = 0;y<branches.Count;y++)
                {
                    Node node = new Node();
                    node.point = new Point(){x=x,y=y};
                    temp.Add(node);
                }
                result.Add(temp);
            }
            return result;
        }
    }
    public class Node
    {
        public Point point;
        public string type;
    }
    [Serializable]
    public class Point
    {
        public int x;
        public int y;
        public static Tree FindWithPoint(List<Tree> tree, Point point)
        {
            var test = tree.Find((tree) => tree.point.x == point.x&&tree.point.y == point.y);
            return test;
        }
    }
}