#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossTeleportState : BossState
    {
        bool startTeleport;
        public BossTeleportState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            startTeleport = false;
            machine.theBoss.SetAnimatorTrigger("initTeleport");
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (elapsedTime > 1.5f && startTeleport == false)
            {
                startTeleport = true;
                machine.theBoss.SetAnimatorBool("canTeleport", true);
                machine.theBoss.PerformTeleport();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            machine.theBoss.SetAnimatorBool("canTeleport", false);
            elapsedTime = 0;
        }
    }
}