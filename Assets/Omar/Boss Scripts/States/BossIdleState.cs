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
            base.OnEnter();
            machine.theBoss.SetAnimatorSpeed(0);
            machine.theBoss.SetAgentSpeed(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(elapsedTime > 3.0f && machine.theBoss.GetDistanceFromPlayer() > 2.5f)
            {
                machine.ChangeState(new BossPursueState(machine));
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            elapsedTime = 0;
        }
    }
}