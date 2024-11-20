using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossIdleState : BossState
    {
        public BossIdleState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            Debug.Log("entered idle");
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            Debug.Log("update idle");
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}