using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Control.Core;
using DataContainer;

namespace Map
{
    public class MapHandler : MonoBehaviour
    {
        Tree MainTree;
        public float VerticalSep;
        public float HorizontalSep;
        public GameObject NodePrefab;
        public GameObject DotPrefab;
        public float DotGap;
        public GameObject Character;
        NodeHandler InitializedMainTree;
        List<Tree> CurrentTree = null;
        public float moveDuration;
        public NodeHandler CurrentPlayerPos;
        private void ClearBoard()
        {
            for(int i = 0;i<gameObject.transform.childCount;i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        void Awake()
        {
            if (LoadedSave.Loaded.act[LoadedSave.Loaded.currAct].tree == null)
            {
                this.Spawn();
                LoadedSave.Loaded.act[LoadedSave.Loaded.currAct].tree = this.CurrentTree;
                SaveFile.Save(LoadedSave.Loaded);
            }else
            {
                this.Spawn(LoadedSave.Loaded.act[LoadedSave.Loaded.currAct].tree);
                if (LoadedSave.Loaded.LastLevelWin)
                {
                    this.CurrentPlayerPos.ProceedNode();
                    if (this.CurrentPlayerPos.Child.Count == 0)
                    {
                        // LoadedSave.Loaded.act[LoadedActData.CurrAct].finished = true;
                        // ToActChooseScreen.LoadScene();
                        ActDataLoader.nextAct();
                        ChangeScene.LoadActMap();
                        LoadedSave.Loaded.LastLevelWin = false;
                    }
                }
            }
        }
        public void Spawn(List<Tree> tree = null)
        {
            if (tree == null)
            {
                this.CurrentTree = TreeGenerator.Generate(TargetLenp:ActDataLoader.loadedActData.Length);
                this.CurrentTree[0].CurrentPlayerPos = true;
                LoadedSave.Loaded.act[LoadedSave.Loaded.currAct].tree = this.CurrentTree;
                Debug.Log("generated len:" + this.CurrentTree[this.CurrentTree.Count-1].point.x.ToString());
            } else
            {
                this.CurrentTree = tree;
            }
            this.DrawTree(this.CurrentTree);
            this.InitializedMainTree = this.CurrentTree[0].SelfInstance;
            //enable the parent tree as starter
            // this.InitializedMainTree.ProceedNode();
            this.InitializedMainTree.Active = true;
            // so the movement is instant on load
            float tmp = this.moveDuration;
            this.moveDuration = 0;
            this.ProgressPosition(this.CurrentPlayerPos);
            this.moveDuration = tmp;
        }
        public void ProgressPosition(NodeHandler Caller)
        {
            Character.GetComponent<CharacterController>().AddMove(Character.transform.position, this.moveDuration);
            float delta = Caller.transform.position.x - Character.transform.position.x;
            this.transform.DOMoveX(this.transform.position.x - delta, this.moveDuration).OnComplete(()=>Caller.Active = true);
        }
        public void DrawTree(List<Tree> tree)
        {
            ClearBoard();
            List<Tree> CurrBranch = new List<Tree>(){tree[0]};
            int Collumn = 1;
            while (Collumn<tree.Count)
            {
                List<Tree> Temp = new List<Tree>();
                this.DrawTreeRow(tree, CurrBranch, (Collumn-1)*HorizontalSep);
                foreach(Tree Branch in CurrBranch)
                {
                    if(Branch.Child.Count<= 0)break;
                    foreach (Point point in Branch.Child)
                    {
                        Temp.Add(Point.FindWithPoint(tree, point));
                    }
                }
                CurrBranch = Temp.Distinct().ToList();
                Collumn++;
            }
        }
        private void DrawTreeRow(List<Tree> WholeTree, List<Tree> Row, float X)
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
                Script.tree = Branch;
                if (Branch.CurrentPlayerPos)
                {
                    this.CurrentPlayerPos = Script;
                    Script.Active = true;
                }
                if(Branch.Parent.Count>0)
                {
                    Script.Init(Branch.Type, Branch.Parent.Select(tree => Point.FindWithPoint(WholeTree,tree).SelfInstance).ToList());
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
