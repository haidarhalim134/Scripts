using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;
using Random = System.Random;
using Control.Core;

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
                {"normal", new int[] {6}},
                {"branch", new int[] {3}},
                {"join", new int[] {0}}
            }},
            {"node", new Dictionary<string, int[]>(){
                {"Home", new int[] {0}},
                {"Enemy", new int[] {8}},
                {"Event", new int[] {0}}
            }}
        };
        static Dictionary<string, NodeType> Convert = new Dictionary<string, NodeType>(){
            {"Home",NodeType.Home},
            {"Enemy",NodeType.Enemy},
            {"Event",NodeType.Event},
        };
        public static float BranchValue(int count)
        {
            return (count-1) * 30f;
        }
        public static int TargetLen;
        public static int MaxWidth;
        public static int MinWidth;
        public static int CurrLen;
        public static Tree MainTree;
        public static List<Tree> CurrentBranches = new List<Tree>();
        public static Tree Generate(int TargetLenp = 10, int MaxWidthp = 7, int MinWidthp = 4) 
        {
            TargetLen = TargetLenp;
            MaxWidth = MaxWidthp;
            MinWidth = MinWidthp;
            CurrLen = 1;
            MainTree = new Tree();
            MainTree.Init(NodeType.Enemy, NodeValue[NodeType.Enemy]);
            CurrentBranches.Clear();
            CurrentBranches.Add(MainTree);
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
                List<string> Nodes = rnd.Choice(new List<string>(Tendencies["node"].Keys), CalcNodeWeights(),
                 PreNode=="normal"?1:2);
                foreach (NodeType node in Nodes.Select((a)=>Convert[a]))
                {
                    Tree NewNode = new Tree();
                    NewNode.Init(node, BranchValue(Nodes.Count) + NodeValue[node], branch);
                    if(QParent!= null)
                    {
                        NewNode.Parent.Add(QParent);
                        QParent.Child.Add(NewNode);
                        QParent = null;
                    }
                    branch.Child.Add(NewNode);
                    NextBranches.Add(NewNode);
                }
            }
            CurrentBranches = NextBranches;
            CurrLen+= 1;
            MainTree.Length = CurrLen;
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
        private static List<int> CalcNodeWeights()
        {
            int wHome = CalcWeightIndex(Tendencies[Node]["Home"]);
            int wEnemy = CalcWeightIndex(Tendencies[Node]["Enemy"]);
            int wEvent = CalcWeightIndex(Tendencies[Node]["Event"]);
            return new List<int>(){wHome, wEnemy, wEvent};
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
        public NodeType Type;
        [NonSerialized]
        public List<Tree> Parent = new List<Tree>();
        public List<Tree> Child = new List<Tree>();
        public float PathValue;
        public int Length;
        [NonSerialized]
        public NodeHandler SelfInstance;
        public void Init(NodeType Type, float InitValue = 0, Tree Parent = null, Tree Child = null)
        {
            this.Type = Type;
            this.PathValue = InitValue;
            if (Parent != null)
            {
                this.Parent.Add(Parent);
                this.PathValue += Parent.PathValue;
            }
        }
        public void AssignParent(Tree tree = null)
        {
            if (tree == null)
            {
                tree = this;
            }
            if (tree.Child.Count == 0)
            {
                return;
            }
            foreach (Tree child in tree.Child)
            {
                child.Parent.Add(tree);
                child.Parent.Distinct().ToList();
                child.Child.Distinct().ToList();
                this.AssignParent(child);
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
    }
}