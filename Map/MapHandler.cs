using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Map
{
    public class MapHandler : MonoBehaviour
    {
        Tree MainTree;
        public float VerticalSep;
        public float HorizontalSep;
        public GameObject NodePrefab;
        public Material LineMaterial;
        public GameObject Character;
        NodeHandler InitializedMainTree;
        Tree CurrentTree = null;
        private void ClearBoard()
        {
            for(int i = 0;i<gameObject.transform.childCount;i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        public void Spawn(Tree tree = null)
        {
            if (tree == null)
            {
                this.CurrentTree = TreeGenerator.Generate(MaxWidthp: 5);
                Debug.Log("generated len:" + this.CurrentTree.Length.ToString());
            } else
            {
                this.CurrentTree = tree;
            }
            this.DrawTree(this.CurrentTree);
            this.InitializedMainTree = this.CurrentTree.SelfInstance;
            //enable the parent tree as starter
            this.InitializedMainTree.ProceedNode();
            Character.GetComponent<CharacterController>().SetPosition(this.InitializedMainTree.gameObject.transform.position);
        }
        public void Generate()
        {
            this.Spawn();
        }
        public void SaveTree()
        {
            if (this.CurrentTree!= null)
            {
                FileProcessor.WriteToXmlFile<Tree>("B:/tree.xml",this.CurrentTree);
                Debug.Log("saved");
            }
        }
        public void LoadTree()
        {
            Tree tree = FileProcessor.ReadFromXmlFile<Tree>("B:/tree.xml");
            tree.AssignParent();
            this.Spawn(tree);
            Debug.Log("loaded");
        }
        public void ProgressPosition(NodeHandler Caller)
        {
            Character.GetComponent<CharacterController>().AddMove(Caller.gameObject.transform.position);
        }
        public void DrawTree(Tree tree)
        {
            ClearBoard();
            List<Tree> CurrBranch = new List<Tree>(){tree};
            int Collumn = 1;
            while (Collumn<tree.Length)
            {
                List<Tree> Temp = new List<Tree>();
                this.DrawTreeRow(CurrBranch, (Collumn-1)*HorizontalSep);
                foreach(Tree Branch in CurrBranch)
                {
                    if(Branch.Child.Count<= 0)break;
                    Temp.AddRange(Branch.Child);
                }
                CurrBranch = Temp.Distinct().ToList();
                Collumn++;
            }
        }
        private void DrawTreeRow(List<Tree> Row, float X)
        {
            int BranchCount = Row.Count;
            List<float> YPos = CalcYPos(BranchCount);
            int YIndex = 0;
            foreach(Tree Branch in Row)
            {
                GameObject Object = Instantiate(NodePrefab, new Vector2(), new Quaternion(), gameObject.transform);
                Object.GetComponent<RectTransform>().anchoredPosition = new Vector2(X, YPos[YIndex]);
                NodeHandler Script = Object.GetComponent<NodeHandler>();
                Script.mapHandler = this;
                Branch.SelfInstance = Script;
                if(Branch.Parent.Count>0)
                {
                    Script.Init(Branch.Type, Branch.Parent.Select(tree => tree.SelfInstance).ToList());
                }else
                {
                    Script.Init(Branch.Type, null);
                }
                YIndex++;
            }
        }
        private List<float> CalcYPos(int BranchCount, float MidPoint = 0)
        {
            List<float> Positions = new List<float>();
            float point;
            if (BranchCount%2==1)
            {
                Positions.Add(MidPoint);
                decimal length = (BranchCount+1)/2;
                for (int i = 1;i<Math.Floor(length);i++)
                {
                    point = Positions[Positions.Count-1] + this.VerticalSep;
                    Positions.Add(point);
                    point = Positions[0] - this.VerticalSep;
                    Positions.Insert(0, point);
                }
            } 
            else 
            {
                Positions.Add(MidPoint-this.VerticalSep/2);Positions.Add(MidPoint+this.VerticalSep/2);
                decimal length = (BranchCount+1)/2;
                for (int i = 1;i<Math.Floor(length);i++)
                {
                    point = Positions[Positions.Count-1] + this.VerticalSep;
                    Positions.Add(point);
                    point = Positions[0] - this.VerticalSep;
                    Positions.Insert(0, point);
                }
            }
            return Positions;
        }
    }
}
