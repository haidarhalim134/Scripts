using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;
using Random = System.Random;
using Control.Core;
using UnityEngine;
using DataContainer;

namespace Map
{
    public class MapGenerator
    {
        
    }
    public static class TreeGenerator
    {
        private static Random rnd = new Random();
        private static string Node = "node";
        private static string PreNode = "pre_node";
        // TODO: consider creating custom data container, plus a custom config data interface, might require massive refactor
        private static Dictionary<NodeType, float> NodeValue = new Dictionary<NodeType, float>()
        {
            {NodeType.Home, 80f},
            {NodeType.Enemy, 30f},
            {NodeType.Event, 50f},
        };
        private static Dictionary<string, Dictionary<string, int[]>> Tendencies = new Dictionary<string, Dictionary<string, int[]>>()
        {
            {"pre_node", new Dictionary<string, int[]>(){
                {"normal", new int[] {12}},
                {"branch", new int[] {2}},
                {"join", new int[] {1}}
            }},
            {"node", new Dictionary<string, int[]>(){
                {"Home", new int[] {1,2,3,2,1}},
                {"Enemy", new int[] {8}},
                {"Event", new int[] {0}},
                {"Upgrade", new int[] {1, 2, 3, 2, 1 }}
            }}
        };
        static Dictionary<string, NodeType> Convert = new Dictionary<string, NodeType>(){
            {"Home",NodeType.Home},
            {"Enemy",NodeType.Enemy},
            {"Event",NodeType.Event},
            {"Upgrade",NodeType.Upgrade},
        };
        public static float BranchValue(int count)
        {
            return (count-1) * 30f;
        }
        public static int TargetLen;
        public static int MaxWidth;
        public static int MinWidth;
        public static int CurrLen;
        public static int CurrWidth;
        public static List<Tree> MainTree;
        public static List<Tree> CurrentBranches = new List<Tree>();
        public static List<Tree> Generate(int TargetLenp = 10, int MaxWidthp = 7, int MinWidthp = 4) 
        {
            TargetLen = TargetLenp;
            MaxWidth = MaxWidthp;
            MinWidth = MinWidthp;
            CurrLen = 1;
            CurrWidth = 1;
            MainTree = new List<Tree>(){new Tree()};
            MainTree[0].Init(NodeType.Enemy, new Point(){x=CurrLen,y=CurrWidth});
            CurrentBranches.Clear();
            CurrentBranches.Add(MainTree[0]);
            while (CurrLen < TargetLen)
            {
                Grow();
            }
            return MainTree;
        }
        public static void Grow()
        { 
            List<Tree> NextBranches = new List<Tree>();
            Tree QParent = null;
            CurrWidth = 1;
            CurrLen+= 1;
            foreach (Tree branch in CurrentBranches)
            {
                string PreNode;
                if (QParent == null)
                {
                    PreNode = rnd.Choice(new List<string>(Tendencies["pre_node"].Keys),CalcPreNodeWeights())[0];
                    //make sure that the node to join isnt the last node on the queue, else normalize
                    if (PreNode == "join" && !(CurrentBranches.IndexOf(branch) == CurrentBranches.Count-1))
                    {
                        QParent = branch;
                        continue;// send the current branch to next new branch
                    } else if(PreNode == "join")
                    {
                        PreNode = "normal";
                    }
                } else
                {
                    PreNode = "normal";
                }
                List<string> Nodes = rnd.Choice(new List<string>(Tendencies["node"].Keys), Tendencies["node"].Values.ToList().Select(item=>CalcWeightIndex(item)).ToList(),
                 PreNode=="normal"?1:2);
                foreach (NodeType node in Nodes.Select((a)=>Convert[a]))
                {
                    Tree NewNode = new Tree();
                    NewNode.Init(node, new Point() { x = CurrLen, y = CurrWidth }, branch.point);
                    MainTree.Add(NewNode);
                    if(QParent!= null)
                    {
                        NewNode.Parent.Add(QParent.point);
                        QParent.Child.Add(NewNode.point);
                        QParent = null;
                    }
                    branch.Child.Add(NewNode.point);
                    NextBranches.Add(NewNode);
                    CurrWidth += 1;
                }
            }
            CurrentBranches = NextBranches;
        }
        /// <summary>calculate weight for each pre node action chance</summary>
        private static List<int> CalcPreNodeWeights()
        {
            // make sure the return list order is the same as the order in the dict
            int Normal = CalcWeightIndex(Tendencies[PreNode]["normal"]);
            int Branch = CurrentBranches.Count>= MaxWidth? 0:
            (int)Math.Round(CalcWeightIndex(Tendencies[PreNode]["branch"])*MinWidth*1.5/(float)CurrentBranches.Count);
            int Join = CalcWeightIndex(Tendencies[PreNode]["join"]);
            return new List<int>(){Normal, Branch, Join};
        }
        private static int CalcWeightIndex(int[] Weights)
        {
            return Weights[(int)Math.Ceiling((decimal)(CurrLen*(Weights.Length)/(float)TargetLen))-1];
        }
    }
    [Serializable]
    public class Tree
    {
        public bool CurrentPlayerPos;
        public Point point;
        public NodeType Type;
        public List<Point> Parent = new List<Point>();
        public List<Point> Child = new List<Point>();
        [NonSerialized]
        public NodeHandler SelfInstance;
        public void Init(NodeType Type, Point point, Point Parent = null, Point Child = null)
        {
            this.Type = Type;
            this.point = point;
            if (Parent != null)
            {
                this.Parent.Add(Parent);
            }
        }
    }
    static class Utils
    {
       
    }
    static class RandomUtils
    {
        public static List<string> Choice(this Random rnd, List<string> choices, List<int> weights,int k = 1)
        {
            List<string> result = new List<string>();
            for (int j = 0;j<k;j++){
                var cumulativeWeight = new List<int>();
                int last = 0;
                foreach (var cur in weights)
                {
                    last += cur;
                    cumulativeWeight.Add(last);
                }

                int choice = rnd.Next(last);
                int i = 0;
                Console.WriteLine(choice.ToString(), cumulativeWeight.ToString());
                foreach (var cur in choices)
                {
                    if (choice < cumulativeWeight[i])
                    {
                        Console.WriteLine("o "+cur.ToString());
                        choices.Remove(cur);
                        weights.RemoveAt(i);
                        result.Add(cur);
                        break;
                    }
                    i++;
                }
            }
            return result;
        }
        public static List<BotAbContWeight> Choice(this Random rnd, List<BotAbContWeight> choices, List<int> weights, int k = 1)
        {
            List<BotAbContWeight> result = new List<BotAbContWeight>();
            for (int j = 0; j < k; j++)
            {
                var cumulativeWeight = new List<int>();
                int last = 0;
                foreach (var cur in weights)
                {
                    last += cur;
                    cumulativeWeight.Add(last);
                }

                int choice = rnd.Next(last);
                int i = 0;
                Console.WriteLine(choice.ToString(), cumulativeWeight.ToString());
                foreach (var cur in choices)
                {
                    if (choice < cumulativeWeight[i])
                    {
                        Console.WriteLine("o " + cur.ToString());
                        choices.Remove(cur);
                        weights.RemoveAt(i);
                        result.Add(cur);
                        break;
                    }
                    i++;
                }
            }
            return result;
        }
    }
}