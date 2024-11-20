using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossMeleeState : BossState
    {
        public BossMeleeState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}