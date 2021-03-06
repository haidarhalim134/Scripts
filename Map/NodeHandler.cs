using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Control.Core;
using DataContainer;
using LevelManager;

namespace Map
{
    public class NodeHandler : MonoBehaviour
    {
        public NodeType Type;
        public List<NodeHandler> Child = new List<NodeHandler>();
        public List<NodeHandler> Parent = new List<NodeHandler>();
        /// <summary>if node is active choice for player to move</summary>
        public bool Active;
        public MapHandler mapHandler;
        public Tree tree;
        public void Init(NodeType Type, List<NodeHandler> Parent)
        {
            this.Type = Type;
            this.Parent = Parent;
            gameObject.GetComponent<Image>().color =
             this.Type == NodeType.Enemy ? Color.red : this.Type == NodeType.Upgrade ? Color.blue : Color.yellow;
            if (this.Parent != null)
            {
                foreach (NodeHandler parent in this.Parent)
                {
                    parent.Child.Add(this);
                    GameObject Object = new GameObject("Line");
                    Object.transform.SetParent(mapHandler.transform);
                    DotGenerator.Generate(transform.position, parent.transform.position,
                     mapHandler.DotPrefab, Object.transform,mapHandler.DotGap);
                    // var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                    // lineRenderer.transform.SetParent(gameObject.transform.parent);
                    // // lineRenderer.startColor = Color.black;
                    // // lineRenderer.endColor = Color.black;
                    // lineRenderer.startWidth = 0.1f;
                    // lineRenderer.endWidth = 0.1f;
                    // lineRenderer.positionCount = 2;
                    // lineRenderer.useWorldSpace = true;
                    // lineRenderer.textureMode = LineTextureMode.Tile;
                    // // lineRenderer.useWorldSpace = false;
                    // Material material = mapHandler.LineMaterial;
                    // material.mainTextureScale = new Vector2(Vector3.Distance(transform.position,parent.transform.position), 1);
                    // lineRenderer.material = material;
                    // lineRenderer.gameObject.AddComponent<DotRenderer>();
                    // // lineRenderer.material.SetTextureScale("_MainTex", new Vector2(10f, 1));
                    // // lineRenderer.material.mainTextureScale = new Vector2(0.1f, 1f);

                    // //For drawing line in the world space, provide the x,y,z values
                    // lineRenderer.SetPosition(0, transform.position); //x,y and z position of the starting point of the line
                    // lineRenderer.SetPosition(1, parent.transform.position); //x,y and z position of the end point of the line
                    // // DotRenderer.Lines.Add(new Line(){point1=transform.position,point2=parent.transform.position });
                }
            }
        }
        public void HandleClick()
        {
            if (!this.Active)
            {
                return;
            }else if (this.tree.CurrentPlayerPos)
            {
                if (Type == NodeType.Enemy)
                {
                    LevelDataContainer cont = ActDataLoader.loadedActData
                    .GetLevel(NodeType.Enemy, Loaded.loaded.QueuedLevel.GetQueued(this.Type));
                    LevelLoader.LoadLevel(cont);
                }else if (Type == NodeType.Upgrade)
                {
                    ChangeScene.LoadCardUpgradeScene();
                }else
                {
                    ChangeScene.LoadShopScene();
                }
            }else{
                this.tree.CurrentPlayerPos = true;
                mapHandler.CurrentPlayerPos = this;
                mapHandler.ProgressPosition(this, mapHandler.moveDuration);
                // this.SetActive(false);
                this.CloseChoiceNode();
                Loaded.loaded.LastLevelWin = false;
                SaveFile.Save(Loaded.loaded);
            }
        }
        /// <summary>set the activation of all of this node's childs</summary>
        public void ProceedNode(bool to = true)
        {
            foreach (NodeHandler Node in this.Child)
            {
                Node.SetActive(to);
            }
        }
        /// <summary>disable other choice after picking one node as a path</summary>
        public void CloseChoiceNode()
        {
            foreach (NodeHandler parent in this.Parent?? Enumerable.Empty<NodeHandler>())
            {
                parent.ProceedNode(false);
                parent.tree.CurrentPlayerPos = false;
            }
        }
        public void SetActive(bool to)
        {
            this.Active = to;
        }
    }
}
