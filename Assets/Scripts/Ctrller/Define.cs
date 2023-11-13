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
            NormalAttack,//7
            UpAttack,//8
            DwAttack,//9
            AirNormalAttack,//10
            AirDwAttack,//11
            RLAttack,//12
            RunAttack,//13
            KnockOut,//14
            Stun,//15
            End,

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
        
        UpAttack1,//1
        UpAttack2,//2


        End

    }



}