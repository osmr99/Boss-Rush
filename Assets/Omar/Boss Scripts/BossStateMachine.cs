using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossStateMachine
    {
        public BossState currentState;
        public BossScript theBoss;

        public BossStateMachine(BossScript boss)
        {
            theBoss = boss;
        }

        public void Update()
        {
            currentState.OnUpdate();
        }

        public void ChangeState(BossState newState)
        {
            if (currentState != null)
                currentState.OnExit();

            currentState = newState;

            newState.OnEnter();
        }
    }
}
