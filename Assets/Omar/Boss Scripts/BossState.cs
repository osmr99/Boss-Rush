using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossState
    {
        public BossStateMachine machine;
        public float elapsedTime = 0;
        public BossState(BossStateMachine m)
        {
            this.machine = m;
        }
        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {
            elapsedTime += Time.deltaTime;
        }

        public virtual void OnExit()
        {

        }
    }
}
