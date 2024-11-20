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
            //machine.theBoss.SetSpeed(0);
        }

        public override void OnUpdate()
        {
            Debug.Log("update idle");

            base.OnUpdate();

            if(elapsedTime > 2.0f)
            {
                //Vector3 vel = machine.theBoss.GetVelocity();
                //if(vel != Vector3.zero)
                //{
                    //machine.theBoss.SetSpeed(1);
                    //machine.ChangeState(new BossPursueState(machine));
                //}
            }
            
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}