#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossHurtState : BossState
    {
        public BossHurtState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAnimatorTrigger("changePhase");
            machine.theBoss.canTakeDamage(false);
            machine.theBoss.ToggleMeleeDamager(false);
            machine.theBoss.ToggleCollision(true);
            machine.theBoss.ToggleProjectiles(false);
            machine.theBoss.SetAnimatorBool("canProjectile", false);
            machine.theBoss.StopCoroutines();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(elapsedTime > 2.5f)
            {
                
                if(machine.theBoss.GetAnimatorBool("canUlti") == false)
                {
                    machine.theBoss.ToggleCollisionDamager(true);
                    machine.theBoss.canTakeDamage(true);
                    machine.ChangeState(new BossIdleState(machine));
                }
                else
                {
                    machine.ChangeState(new BossLaserState(machine));
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