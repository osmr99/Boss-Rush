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
            machine.theBoss.ResetAnimatorTrigger("canMelee");
            machine.theBoss.ResetAnimatorTrigger("initDash");
            machine.theBoss.ResetAnimatorTrigger("initLaser");
            machine.theBoss.ResetAnimatorTrigger("initCry");
            machine.theBoss.ResetAnimatorTrigger("initTeleport");
            machine.theBoss.SetAnimatorBool("canProjectile", false);
            machine.theBoss.SetAnimatorBool("canDash", false);
            machine.theBoss.SetAnimatorBool("canLaser", false);
            machine.theBoss.SetAnimatorBool("canCry", false);
            machine.theBoss.SetAnimatorBool("canTeleport", false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (machine.theBoss.GetMustIdle() == true)
                return;

            if (elapsedTime > 0.5f)
            {
                if (machine.theBoss.GetDistanceFromPlayer() <= 2.8f && machine.theBoss.GetAnimatorBool("wasOnDash") == true)
                {
                    machine.ChangeState(new BossMeleeState(machine));
                }
                else
                {
                    machine.theBoss.SetAnimatorBool("wasOnDash", false);
                    machine.theBoss.SetAnimatorFloat("speed", 1);
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