using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nara
{

   
        // Start is called before the first frame update
        public enum PlayerState
        {
            Idle,//0
            Running,//1
            Jumping,//2
            DoudbleJumping,//3
            Falling,//4
            Floating,//5
            Die,//6
            Attack,//7
            KnockOut,//8
            Stun,//9

        }

    public enum Effect
    {
        RRun,
        LRun,
        RBreak,
        LBreak,
        RJump,
        LJump,
        DJump,
        Land,
        End
    }

    public enum AtkEffect1
    {
        Normal1,//0
        Normal2,//1
        Normal3,//2
    
        End

    }

}