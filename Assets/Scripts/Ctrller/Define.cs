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
            Stun,//6
            NormalAttack,//7
            NormalAttack2,//8
            NormalAttack3,//9
            UpAttack,//10
            DwAttack,//11
            AirNormalAttack,//12
            AirDwAttack,//13
            RLAttack,//14
            RunAttack,//15
            KnockOut,//16
     
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
        Test,
        End
    }

    public enum AtkEffect1
    {
        
        UpAttack1,//0
        UpAttack2,//1
        RLAttack1,//2
        RLAttack2,//3
        DwAttack1,//4
        DwAttack2,//5
        NormalAttack1,//6
        NormalAttack2,//7
        NormalAttack3,//8

        End,

    }



}