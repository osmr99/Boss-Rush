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
            if (machine.theBoss.GetMeleeDamagerState() == true)
                machine.theBoss.ToggleMeleeDamager(false);
            machine.theBoss.SetAgentOffset(0.83f);
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
                randomNum = Random.Range(0, 101);
                if (machine.theBoss.GetAnimatorInt("currentPhase") == 1)
                {
                    if (randomNum > 50)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossMeleeState(machine));
                    }
                    else if (randomNum <= 50)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossProjectileState(machine));
                    }
                }
                else if (machine.theBoss.GetAnimatorInt("currentPhase") == 2)
                {
                    if(randomNum > 66)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossMeleeState(machine));
                    }
                    else if(randomNum <= 66 && randomNum > 33)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossProjectileState(machine));
                    }
                    else if(randomNum <= 33)
                    {
                        machine.ChangeState(new BossLaserState(machine));
                    }
                }
                else if(machine.theBoss.GetAnimatorInt("currentPhase") == 3)
                {
                    if(randomNum > 60)
                    {
                        machine.ChangeState(new BossLaserState(machine));
                    }
                    else if(randomNum <= 60 && randomNum > 40)
                    {
                        machine.ChangeState(new BossTeleportState(machine));
                    }
                    else if(randomNum <= 40 && randomNum > 20)
                    {
                        machine.ChangeState(new BossWarcryState(machine));
                    }
                    else if(randomNum <= 20 && randomNum > 10)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossProjectileState(machine));
                    }
                    else if (randomNum <= 10)
                    {
                        machine.theBoss.LookAtPlayer();
                        machine.ChangeState(new BossMeleeState(machine));
                    }
                }
            }
            if(elapsedTime > 3.0f)
            {
                randomNum = Random.Range(0, 101);
                if(machine.theBoss.GetAnimatorInt("currentPhase") == 1)
                {
                    if (randomNum > 50)
                    {
                        machine.ChangeState(new BossDashState(machine));
                    }
                    else if (randomNum <= 50)
                    {
                        machine.ChangeState(new BossProjectileState(machine));
                    }
                }
                else if(machine.theBoss.GetAnimatorInt("currentPhase") == 2)
                {
                    if(randomNum > 66)
                    {
                        machine.ChangeState(new BossLaserState(machine));
                    }
                    else if (randomNum <= 66 && randomNum > 33)
                    {
                        machine.ChangeState(new BossDashState(machine));
                    }
                    else if (randomNum <= 33)
                    {
                        machine.ChangeState(new BossProjectileState(machine));
                    }
                }
                else if(machine.theBoss.GetAnimatorInt("currentPhase") == 3)
                {
                    if (randomNum > 60)
                    {
                        machine.ChangeState(new BossLaserState(machine));
                    }
                    else if (randomNum <= 60 && randomNum > 40)
                    {
                        machine.ChangeState(new BossTeleportState(machine));
                    }
                    else if (randomNum <= 40 && randomNum > 20)
                    {
                        machine.ChangeState(new BossWarcryState(machine));
                    }
                    else if (randomNum <= 20)
                    {
                        machine.ChangeState(new BossProjectileState(machine));
                    }
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