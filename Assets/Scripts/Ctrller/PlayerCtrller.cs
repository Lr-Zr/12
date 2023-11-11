using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine.Playables;
using System.IO.IsolatedStorage;

namespace nara
{

    public class PlayerCtrller : MonoBehaviour
    {


        //이동속도
        [SerializeField]
        float _MoveSpeed = 10.0f;

        //점프력
        [SerializeField]
        float _JumpingPower = 10.0f;

        //게이지
        [SerializeField]
        float _Gauge = 1.0f;

        //목숨
        [SerializeField]
        int _Life = 3;

        //미끄러지는 속도
        [SerializeField]
        float _SlideSpeed = 50.0f;

        //하강키 누를때 스피드
        [SerializeField]
        float _DownSpeed = 3.0f;

        //점프 후  행동제약
        [SerializeField]
        float _JumpRestriction = 0.25f;

        //관성 시작 시간
        [SerializeField]
        float _RunRestriction = 1.0f;

        //전진 공격 가능 시간
        [SerializeField]
        float _AttackRestriction = 0.75f;

        //관성시간
        [SerializeField]
        float _SlideTime = 0.25f;

        [SerializeField]
        float _AtkStopTime = 0.25f;

        //파워
        [SerializeField]
        Vector3[] _Power;



        Rigidbody _Rigid;
        PlayerAnimation _Anim;
        PlayerEffect _Eff;
        PlayerState _State;
        //방향 좌우 move함수용
        int dir;
        //time
        public float _RunTime = 0.0f;
        float _JumpTime = 0.3f;
        float _Floortime = 0.0f;//
        float _AttackTime = 0.5f;


        bool _IsAttack;
        bool _IsJump;
        bool _IsDJump;
        bool _IsOnesec;//질주 공격 
        bool _IsDashAtk;
        bool _IsRunning;
        bool _Stop;


        //keydown time;
        float _KDwTime = 0.2f;
        float _KUpTime = 0.2f;

        //keydown bool;
        bool _IsKeyDown;
        bool _IsKeyUp;


        Vector3 _PrePos;
        void Start()
        {

            _Rigid = GetComponent<Rigidbody>();
            _Anim = GetComponent<PlayerAnimation>();
            _Eff = GetComponent<PlayerEffect>();

            GameMgr.Input.KeyAction -= OnKeyboard;
            GameMgr.Input.KeyAction += OnKeyboard;
            _State = PlayerState.Idle;
            _IsJump = false;
            _IsDJump = false;
            _IsOnesec = false;
            _IsRunning = false;
            _IsKeyDown = false;
            _IsKeyUp = false;
            _IsAttack = false;

        }

        private void FixedUpdate()
        {
            //달리다가 멈추는 조건
            _Floortime += Time.deltaTime;

            //공격을 한 이후 공격 판정이 꺼지는 시간
            _AttackTime += Time.deltaTime;
            if (_AttackTime > _AtkStopTime)
            {
                _IsAttack = false;
                _Anim.SetIsAttack(_IsAttack);
            }
            //커맨드 키입력
            _KUpTime -= Time.deltaTime;
            _KDwTime -= Time.deltaTime;
            if (_KUpTime < 0) _IsKeyUp = false;
            if (_KDwTime < 0) _IsKeyDown = false;


            //체공 시간
            if (_IsJump)
                _JumpTime += Time.deltaTime;

            //브레이킹 질주 공격 조건 RunTime;
            if (_RunTime > _RunRestriction)
            {
                _RunTime = _RunRestriction;
                _IsOnesec = true;
            }

            if (_RunTime > _RunRestriction)
            {
                _IsDashAtk = true;
            }
            else
                _IsDashAtk = false;
            //낙하상태 
            if (_Rigid.velocity.y < -0.05f && _IsJump)//낙하
            {
                _State = PlayerState.Falling;
                _Anim.SetAnim(_State);

            }
            
            
        }
        void Update()
        {
            //바닥에 있는지 체크 함수;
            OnFloor();
            //브레이킹 부분
            Breaking();


        }

        void OnKeyboard()
        {
            //if (!_Pv.IsMine) return;

            if (Input.GetKey(KeyCode.DownArrow))//조합기 하 및 하강 속도 향상
            {
                _IsKeyDown = true;
                _IsKeyUp = false;
                _KDwTime = 0.2f;

                if (_IsJump && _JumpTime > _JumpRestriction)
                    Move(0);
            }

            else if (Input.GetKey(KeyCode.UpArrow))//조합키 상
            {
                _KUpTime = 0.2f;
                _IsKeyDown = false;
                _IsKeyUp = true;


            }


            if (Input.GetKey(KeyCode.LeftArrow))//좌 이동
            {
                /*이동 및 방향전환*/

                /* 땅에서 달릴 때 */

                if (!_IsJump)
                {
                    _RunTime += Time.deltaTime;
                    dir = -1;
                    _IsRunning = true;
                    _State = PlayerState.Running;
                    _Anim.SetAnim(_State);

                    _Eff.EffectPlay(Effect.LRun);
                }
                Move(-1);

            }
            if (Input.GetKey(KeyCode.RightArrow))//우 이동
            {

                /* 땅에서 달릴 때 */
                if (!_IsJump)
                {
                    _IsRunning = true;
                    _State = PlayerState.Running;
                    _Anim.SetAnim(_State);

                    dir = 1;
                    _RunTime += Time.deltaTime;
                    _Eff.EffectPlay(Effect.RRun);
                }
                /*이동 및 방향전환*/
                Move(1);

            }





            if (Input.GetKey(KeyCode.Q)) //공격
            {
                _AttackTime = 0;
                _Anim.TriggerAtk();

                _IsAttack = true;
                _Anim.SetIsAttack(_IsAttack);

                
            }

            else if (Input.GetKey(KeyCode.W))//스킬
            {

            }

            else if (Input.GetKey(KeyCode.E))//방어
            {

            }

            else if (Input.GetKey(KeyCode.R))//잡기
            {

            }






            /* 점프 */
            if (Input.GetKey(KeyCode.Space))
            {
                _RunTime = 0;
                if (!_IsJump && _State != PlayerState.Falling)
                {
                    Jump();
                    _State = PlayerState.Jumping;
                    _IsJump = true;

                    _Anim.SetAnim(_State);
                    _Anim.SetIsJump(_IsJump);
                    _JumpTime = 0;

                    if (dir > 0)
                        _Eff.EffectPlay(Effect.RJump);
                    else
                        _Eff.EffectPlay(Effect.LJump);
                }
                else if (!_IsDJump && _JumpTime > 0.25)
                {
                    Jump();
                    _State = PlayerState.DoudbleJumping;
                    _IsDJump = true;
                    _Anim.SetAnim(_State);
                    _Anim.SetIsDJump(_IsDJump);
                    _JumpTime = 0;

                    _Eff.EffectPlay(Effect.DJump);
                }

                _IsRunning = false;

            }










        }


        void Jump()
        {
            _Rigid.velocity = Vector3.zero;
            _Rigid.AddForce(Vector3.up * _JumpingPower, ForceMode.Impulse);
        }






        void Move(int dir)      /*이동 및 방향전환*/
        {

            if (!_IsJump)
            {
                if (dir == 0) return;
                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                this.transform.position += transform.forward * _MoveSpeed * Time.deltaTime;
            }
            else
            {
                if (dir == 0)
                {
                    this.transform.position -= Vector3.up * _DownSpeed * Time.deltaTime;

                }
                else
                {
                    this.transform.position += Vector3.right * dir * _MoveSpeed / 2.0f* Time.deltaTime;

                }
            }

        }




        void Breaking()
        {
            //달리다가 멈추면 미끄러짐
            if (_IsOnesec)//탄성 효과
            {
                _RunTime -= Time.deltaTime;
                if (_RunTime > _RunRestriction - _SlideTime)
                {
                    //Debug.Log("이동");
                    _Rigid.AddForce(this.transform.forward * _SlideSpeed);
                }
                else
                {
                    _RunTime = 0f;
                    _IsRunning = false;
                    _IsOnesec = false;
                    if (dir > 0)
                        _Eff.EffectPlay(Effect.RBreak);
                    else
                        _Eff.EffectPlay(Effect.LBreak);
                }

            }
            _PrePos = transform.position;
        }

        void OnFloor()
        {
            RaycastHit hit;
            Vector3 StartPos = transform.position;
            StartPos.y += 0.1f;
            Debug.DrawRay(StartPos, this.transform.up * -0.2f, Color.green);
            LayerMask mask = LayerMask.GetMask("Floor");
            if (Physics.Raycast(StartPos, this.transform.up * -1, out hit, 0.2f, mask))
            {
                //&& _Floortime > 0.05
                if (_State != PlayerState.Idle && _JumpTime > _JumpRestriction  )
                {
                    if (_IsJump)
                        _Eff.EffectPlay(Effect.Land);
                    _Rigid.velocity = Vector3.zero;
                    _Floortime = 0.0f;

                    _State = PlayerState.Idle;
                    _Anim.SetAnim(_State);

                    _IsRunning = false;
                    _IsJump = false;
                    _IsDJump = false;
                    _Anim.SetIsJump(_IsJump);
                    _Anim.SetIsDJump(_IsDJump);

                    Debug.Log("check");
                   
                }

            }
        }




    }
}



