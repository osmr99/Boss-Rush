#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossMeleeState : BossState
    {

        int randomNum;
        int lastMelee;
        public BossMeleeState(BossStateMachine m) : base(m)
        {
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            //Debug.Log(machine.currentState);
            machine.theBoss.SetAnimatorFloat("speed", 0);
            machine.theBoss.SetAgentSpeed(0);
            machine.theBoss.SetAnimatorTrigger("canMelee");
            lastMelee = machine.theBoss.GetAnimatorInt("lastMeleeChoice");
            if(lastMelee == -1)
                randomNum = Random.Range(0, 3); //0, 3
            else
            {
                while (randomNum == lastMelee)
                {
                    randomNum = Random.Range(0, 3); //0, 3
                }
            }
            switch(randomNum)
            {
                case 0:
                    machine.theBoss.SetAnimatorInt("meleeChoice", randomNum);
                    machine.theBoss.SetAnimatorInt("lastMeleeChoice", randomNum);
                    machine.theBoss.StartCoroutineMeleeOne();
                    break;
                case 1:
                    machine.theBoss.SetAnimatorInt("meleeChoice", randomNum);
                    machine.theBoss.SetAnimatorInt("lastMeleeChoice", randomNum);
                    machine.theBoss.StartCoroutineMeleeTwo();
                    break;
                case 2:
                    machine.theBoss.SetAnimatorInt("meleeChoice", randomNum);
                    machine.theBoss.SetAnimatorInt("lastMeleeChoice", randomNum);
                    machine.theBoss.StartCoroutineMeleeThree();
                    break;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();

            elapsedTime = 0;
            machine.theBoss.SuccessfulMeleeEnd();
        }
    }
}