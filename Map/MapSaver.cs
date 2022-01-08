using System;
using System.Linq;
using System.Collections.Generic;

namespace Map
{
    public class MapSaver
    {
        public List<Node> ConvertToNode(Tree tree)
        {
            int x = 0;
            List<Tree> branches = new List<Tree>(){tree};
            List<Node> result = new List<Node>();
            while (branches.Count>0)
            {
                for (var y = 0;y<branches.Count;y++)
                {
                    Node node = new Node();
                    result.Add(node);
                    node.point = new Point(){x=x,y=y};
                }
            }
            return result;
        }
    }
    public class Node
    {
        public Point point;
        public List<Point> parent;
        public List<Point> children;
        public string type;
    }
    public class Point
    {
        public int x;
        public int y;
    }
}