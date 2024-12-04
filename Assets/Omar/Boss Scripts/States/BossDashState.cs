using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossDashState : BossState
    {
        public BossDashState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            //machine.theBoss.AgentResetPath();
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAgentStoppingDistance(10.0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            machine.theBoss.SetAnimatorBool("canDash", true);
            if (elapsedTime > 1.0f)
            {
                machine.theBoss.SetAnimatorBool("canDash", false);
                machine.theBoss.SetAgentSpeed(12);
                machine.theBoss.StartDash();
                machine.ChangeState(new BossIdleState(machine));

            }
        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}