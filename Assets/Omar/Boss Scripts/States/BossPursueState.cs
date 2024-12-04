#pragma warning disable IDE0051
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
            base.OnEnter();

            //Debug.Log(machine.currentState);
            machine.theBoss.SetAnimatorFloat("speed", 1);
            machine.theBoss.SetAgentSpeed(4);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            machine.theBoss.AgentSetPathToPlayer();
            if (machine.theBoss.GetDistanceFromPlayer() <= 2.8f)
            {
                machine.ChangeState(new BossMeleeState(machine));
            }
            if(elapsedTime > 3.0f)
            {
                machine.ChangeState(new BossDashState(machine));
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}