#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossLaserState : BossState
    {
        float delay;
        bool gotDelay;
        bool performedLaser;
        public BossLaserState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            performedLaser = false;
            gotDelay = false;
            machine.theBoss.SetAnimatorTrigger("initLaser");
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.PerformBeam();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(elapsedTime < 0.25f && gotDelay == false)
            {
                gotDelay = true;
                delay = machine.theBoss.GetDelayFloat() - 0.8f;
                //Debug.Log("done");
            }
            else if(gotDelay)
            {
                if (elapsedTime > delay && performedLaser == false)
                {
                    Debug.Log("Laser state delay: " + delay);
                    performedLaser = true;
                    machine.theBoss.SetAnimatorBool("canLaser", true);
                }
                else if (performedLaser == false)
                {
                    machine.theBoss.LookAtPlayer();
                }
            }
                

        }

        public override void OnExit()
        {
            base.OnExit();

            machine.theBoss.SetAnimatorBool("canLaser", false);
            elapsedTime = 0;
        }
    }
}