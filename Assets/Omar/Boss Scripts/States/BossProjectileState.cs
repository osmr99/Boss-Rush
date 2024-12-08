using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossProjectileState : BossState
    {
        bool projectiles = false;
        public BossProjectileState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAnimatorBool("canProjectile", true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(elapsedTime > 1 && projectiles == false)
            {
                projectiles = true;
                machine.theBoss.StartProjectiles();
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
        }
    }
}