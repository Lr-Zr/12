using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace nara
{

    public class GameMgr : MonoBehaviour
    {
        static GameMgr _Ins;//인스턴스
        public static GameMgr Instance { get { Init();return _Ins; } }



        InputMgr _Input = new InputMgr();
        public static InputMgr Input { get { return Instance._Input; } }

        //EffectMgr _Effect = new EffectMgr();
        //public static EffectMgr Effect { get { return Instance._Effect; } } 


        void Start()
        {
            Init();
        }


        void Update()
        {

            _Input.OnUpdate();
        }
        private void FixedUpdate()
        {
           
        }


        static void Init()
        {
            if(_Ins == null )
            {
                GameObject gameobject = GameObject.Find("@GameMgr");
                if(gameobject == null )
                {
                    gameobject = new GameObject { name = "@GameMgr" };//오브젝트 생성
                    gameobject.AddComponent<GameMgr>();

                }
                DontDestroyOnLoad(gameobject);
                _Ins = gameobject.GetComponent<GameMgr>();
            }
        }
    }

}