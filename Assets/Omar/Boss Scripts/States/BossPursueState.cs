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
            //Debug.Log("entered pursue");
            base.OnEnter();
            machine.theBoss.SetSpeed(1);
            machine.theBoss.SetAgentSpeed(5);
        }

        public override void OnUpdate()
        {
            //Debug.Log("updating pursue");

            base.OnUpdate();

            Vector3 vel = machine.theBoss.GetVelocity();
            if (vel == Vector3.zero && elapsedTime > 1.0f)
            {
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