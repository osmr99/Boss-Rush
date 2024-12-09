#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossWarcryState : BossState
    {
        bool startCry;

        public BossWarcryState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            startCry = false;
            machine.theBoss.SetAnimatorTrigger("initCry");
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (elapsedTime > 1.5f && startCry == false)
            {
                startCry = true;
                machine.theBoss.SetAnimatorBool("canCry", true);
                machine.theBoss.PerformWarcry();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            machine.theBoss.SetAnimatorBool("canCry", false);
            elapsedTime = 0;
        }
    }
}