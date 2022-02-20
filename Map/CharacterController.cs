using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Control.UI;

namespace Map
{
    public class CharacterController : MonoBehaviour
    {
        public Animator animator;
        public void Run(bool State)
        {
            this.animator.SetBool("Run", State);
        }
        /// <summary>use "AddMoveDelta" if target is not transform.position</summary>
        public void AddMove(Vector2 to, float moveDuration)
        {
            this.Run(true);
            Vector2 target = to; //+ new Vector2(0, this.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            this.transform.DOMove(target,moveDuration).SetEase(Ease.Linear).OnComplete(()=>this.Run(false));
        }
        public Vector3 GetDeltaY()
        {
            return new Vector3(0,this.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
        }
        public void SetPosition(Vector3 to)
        {
            this.gameObject.transform.position = to + new Vector3(0,this.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
        }
        public Vector3 GetPosition()
        {
            return this.gameObject.transform.position - new Vector3(0, this.GetComponent<SpriteRenderer>().bounds.size.y / 2, 0);
        }
        void Update()
        {
            
        }
    }
}