#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossPursueState : BossState
    {
        int randomNum = 0;
        public BossPursueState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            randomNum = 0;
            machine.theBoss.SetAnimatorFloat("speed", 1);
            machine.theBoss.SetAgentSpeed(4);
            machine.theBoss.SetAgentStoppingDistance(2.75f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            machine.theBoss.AgentSetPathToPlayer();
            if (machine.theBoss.GetDistanceFromPlayer() <= 2.8f)
            {
                machine.theBoss.LookAtPlayer();
                machine.ChangeState(new BossMeleeState(machine));
            }
            if(elapsedTime > 3.0f)
            {
                randomNum = Random.Range(0, 101);
                Debug.Log(randomNum);
                if(randomNum > 35)
                {
                    Debug.Log("Got dash");
                    machine.ChangeState(new BossDashState(machine));
                }
                else if(randomNum <= 35)
                {
                    Debug.Log("Got Projectiles");
                    machine.ChangeState(new BossProjectileState(machine));
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