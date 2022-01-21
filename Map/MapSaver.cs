using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapSaver
    {
        
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