using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossDashState : BossState
    {
        bool dashing = false;
        public BossDashState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            dashing = false;
            machine.theBoss.SetAnimatorTrigger("initDash");
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAgentStoppingDistance(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (elapsedTime <= 1.0f)
            {
                machine.theBoss.AgentSetPathToPlayer();
            }
            else if (elapsedTime > 1.0f && dashing == false)
            {
                dashing = true;
                machine.theBoss.ToggleDashDamager(true);
                machine.theBoss.SetAgentSpeed(12);
                machine.theBoss.StartDash();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}