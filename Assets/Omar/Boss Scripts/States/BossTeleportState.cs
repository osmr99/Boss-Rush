#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossTeleportState : BossState
    {
        public BossTeleportState(BossStateMachine m) : base(m)
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

            Debug.Log("Got teleport...");
            machine.theBoss.ChangeStateToIdle();
        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}