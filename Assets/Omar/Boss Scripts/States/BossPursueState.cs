using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossPursueState : BossState
    {
        public BossPursueState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            Debug.Log("entered pursue");
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            Debug.Log("updating pursue");
            base.OnUpdate();
        }

        public override void OnExit()
        {

            base.OnExit();
        }
    }
}