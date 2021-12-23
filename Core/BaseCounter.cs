using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Control.Core
{
    public class BaseCounter : MonoBehaviour
    {
        public GameObject Target;
        public BaseCreature Creature;
        public TextMeshProUGUI Counter;

        public void Awoke()
        {
            this.Counter = this.GetComponent<TextMeshProUGUI>();
            this.Creature = this.Target.GetComponent<BaseCreature>();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}