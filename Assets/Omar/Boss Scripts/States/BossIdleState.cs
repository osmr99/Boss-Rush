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
            Debug.Log("entered idle");
            base.OnEnter();
            machine.theBoss.SetSpeed(0);
            machine.theBoss.SetAgentSpeed(0);
        }

        public override void OnUpdate()
        {
            Debug.Log("update idle");

            base.OnUpdate();

            if(elapsedTime > 3.0f)
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