using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class BossState
    {
        public BossStateMachine machine;
        public BossState(BossStateMachine m)
        {
            this.machine = m;
        }
        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}
