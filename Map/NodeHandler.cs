using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class NodeHandler : MonoBehaviour
    {
        public string Type;
        public List<NodeHandler> Child = new List<NodeHandler>();
        public List<NodeHandler> Parent = new List<NodeHandler>();
        /// <summary>if node is active choice for player to move</summary>
        public bool Active;
        public MapHandler mapHandler;
        public void Init(string Type, List<NodeHandler> Parent)
        {
            this.Type = Type;
            this.Parent = Parent;
            gameObject.GetComponent<Image>().color =
             this.Type == "enemy" ? Color.red : this.Type == "event" ? Color.blue : Color.yellow;
            if (this.Parent != null)
            {
                foreach (NodeHandler parent in this.Parent)
                {
                    parent.Child.Add(this);
                    var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                    lineRenderer.transform.SetParent(gameObject.transform.parent);
                    // lineRenderer.startColor = Color.black;
                    // lineRenderer.endColor = Color.black;
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.textureMode = LineTextureMode.Tile;
                    // Material material = mapHandler.LineMaterial;
                    // material.mainTextureScale = new Vector2(10f, 1);
                    // lineRenderer.material = material;
                    // lineRenderer.material.SetTextureScale("_MainTex", new Vector2(10f, 1));
                    // lineRenderer.material.mainTextureScale = new Vector2(0.1f, 1f);

                    //For drawing line in the world space, provide the x,y,z values
                    lineRenderer.SetPosition(0, transform.position); //x,y and z position of the starting point of the line
                    lineRenderer.SetPosition(1, parent.transform.position); //x,y and z position of the end point of the line
                }
            }
        }
        public void HandleClick()
        {
            if (!this.Active)
            {
                return;
            }
            mapHandler.ProgressPosition(this);
            // this.SetActive(false);
            this.CloseChoiceNode();
            this.ProceedNode();
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
            foreach (NodeHandler parent in this.Parent)
            {
                parent.ProceedNode(false);
            }
        }
        public void SetActive(bool to)
        {
            this.Active = to;
        }
    }
}
