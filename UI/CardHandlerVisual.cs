using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Attributes.Abilities;
using DG.Tweening;

namespace Control.UI
{
    public class CardHandlerVisual : MonoBehaviour
    {
            public AbilityContainer Ability;
            protected bool MouseOnCard;
            protected List<Move> MoveTarget = new List<Move>();
            private TextMeshProUGUI NameTXT;
            private TextMeshProUGUI CostTXT;
            Sequence sequence;
            public void UpdateText()
            {
               AbilityManager Mng = this.Ability.GetManager();
               TextMeshProUGUI[] txtlist =  GetComponentsInChildren<TextMeshProUGUI>();
               this.CostTXT = txtlist[0];
               this.NameTXT = txtlist[1];
               this.CostTXT.text = Mng.cost.ToString();
               this.NameTXT.text = this.Ability.name;
               txtlist[2].text = Mng.GetDesc();
            }
            public void Destroy() 
            {
                this.DOKill();
                Destroy(gameObject);
            }
    }
    /// <summary>
    ///initialize with current position, target and duration, call "finished" function every update,
    /// if it return false call "tick"
    ///</summary>
    public class Move
    {
        public Vector2 from;
        public Vector2 to;
        /// <summary>in second</summary>
        public float duration;
        float progress;
        public Move(Vector2 from, Vector2 to, float duration)
        {
            this.from = from;
            this.to = to;
            this.duration = duration;
        }
        public bool Finished()
        {
            return duration<=progress;
        }
        public Vector2 Tick()
        {
            float deltatime = Time.deltaTime;
            progress+= deltatime;
            if (progress>=duration)
            {
                return to;
            }
            return Vector2.Lerp(from, to, progress/duration);
        }
    }
}
