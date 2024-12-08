#pragma warning disable IDE0051
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

            //Debug.Log(machine.currentState);
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAnimatorInt("meleeChoice", -1);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (machine.theBoss.GetMustIdle() == true)
                return;

            if (elapsedTime > 0.5f)
            {
                if (machine.theBoss.GetDistanceFromPlayer() <= 2.7f)
                {
                    machine.theBoss.LookAtPlayer();
                    machine.ChangeState(new BossMeleeState(machine));
                }
                else
                {
                    machine.ChangeState(new BossPursueState(machine));
                }
            }


        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}