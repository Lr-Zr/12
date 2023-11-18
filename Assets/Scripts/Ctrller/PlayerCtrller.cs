using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine.Playables;
using System.IO.IsolatedStorage;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;
using TMPro;

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


        //관성시간
        [SerializeField]
        float _BreakingTime = 0.25f;
        //파워
        [SerializeField]
        public Vector3 _Power = Vector3.zero;

        [SerializeField]
        public float Dmg = 0.0f;

        //전진공격 스피드
        [SerializeField]
        float RLspeed = 1000f;


        Rigidbody _Rigid;
        PlayerAnimation _Anim;
        PlayerEffect _Eff;
        public PlayerState _State;
        //방향 좌우 move함수용 및 이펙트 방향 변환
        public int dir;
        public int airdir;
        //time
        public float _RunTime = 0.0f;
        float _BreakTime = 0.0f;
        float _JumpTime = 0.3f;
        float _Floortime = 0.0f;//



        public bool _IsAttack;
        bool _IsJump;
        bool _IsDJump;
        bool _IsOnesec;//질주 공격 
        public bool _IsRunning;


        /* 공격 스킬 관련 변수 */
        public bool _IsRLMove;//전진공격 이동시작
        public bool _IsAirAtk;//중력0으로
        //keydown time;
        float _KDwTime = 0f;
        float _KUpTime = 0f;

        //keydown bool;
        bool _IsKeyDown;
        bool _IsKeyUp;

        //거리 측정용 
        Vector3 _RLMovePos;
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
            _IsRLMove = false;
            _IsAirAtk = false;
        }

        private void FixedUpdate()
        {
            //달리다가 멈추는 조건
            _Floortime += Time.deltaTime;


            if (_IsAirAtk)
                _Rigid.velocity = Vector3.zero;

            if (_IsRLMove)//거리가 어느이상되면 멈춘다.
            {
                float x = Vector3.Distance(_PrePos, _RLMovePos);
                if (x < 3.0f)
                    _Rigid.AddForce(this.transform.forward * RLspeed, ForceMode.Force);
            }


            //커맨드 키입력
            _KUpTime -= Time.deltaTime;
            _KDwTime -= Time.deltaTime;
            if (_KUpTime < 0) _IsKeyUp = false;
            else _IsKeyUp = true;
            if (_KDwTime < 0) _IsKeyDown = false;
            else _IsKeyDown = true;
            _Anim.SetIsUpKey(_IsKeyUp);
            _Anim.SetIsDwKey(_IsKeyDown);

            //체공 시간
            if (_IsJump)
                _JumpTime += Time.deltaTime;

            //브레이킹 질주 공격 조건 RunTime;
            if (_RunTime > _RunRestriction && !_IsJump)
            {
                _RunTime = _RunRestriction;
                _IsOnesec = true;
                _BreakTime = _BreakingTime;
            }
            //달리는지 달리지 않는지 판단.

            if (_RunTime > 0) _IsRunning = true;
            else _IsRunning = false;
            _Anim.SetIsRunning(_IsRunning);


            //Runtime 초기화;
            if (_State != PlayerState.Running && !_IsAttack && !_IsOnesec)
            {
                _RunTime = 0.0f;
            }


            //낙하상태 
            if (_Rigid.velocity.y < -0.05f && _IsJump && !_IsAttack)//낙하
            {
                SetState(PlayerState.Falling);

            }


        }
        void Update()
        {
            //바닥에 있는지 체크 함수;
            OnFloor();
            //브레이킹 부분
            Breaking();

        }
        private void LateUpdate()
        {
            //_Eff.EffectPlay(Effect.Test);
        }
        void OnKeyboard()
        {

            /* 상하 입력 */
            if (Input.GetKey(KeyCode.DownArrow))//조합기 하 및 하강 속도 향상
            {

                _KUpTime = 0.0f;
                _KDwTime = 0.2f;
                if (_IsJump && _JumpTime > _JumpRestriction)
                    Move(0);
            }

            else if (Input.GetKey(KeyCode.UpArrow))//조합키 상
            {
                _KUpTime = 0.2f;
                _KDwTime = 0.0f;


            }

            /* 이동 입력 */
            if (Input.GetKey(KeyCode.LeftArrow))//좌 이동
            {
                if (!_IsAttack)
                {
                    dir = -1;

                    Move(dir);
                }

            }
            if (Input.GetKey(KeyCode.RightArrow))//우 이동
            {
                if (!_IsAttack)
                {
                    dir = 1;
                    Move(dir);

                }

            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                Attack();
            }
            //else if (Input.GetKey(KeyCode.Q)) //공격
            //{

            //}

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
                Jump();

            }
        }




        void Jump()
        {
            if (_IsAttack) return;
            _RunTime = 0;
            _IsOnesec = false;


            if (!_IsJump && _State != PlayerState.Falling)
            {
                _Rigid.velocity = Vector3.zero;
                _Rigid.AddForce(Vector3.up * _JumpingPower, ForceMode.Impulse);

                _JumpTime = 0;
                SetState(PlayerState.Jumping);
                _IsJump = true;
                _Anim.SetIsJump(_IsJump);

                if (dir > 0)
                    _Eff.EffectPlay(Effect.RJump);
                else
                    _Eff.EffectPlay(Effect.LJump);
            }
            else if (!_IsDJump && _JumpTime > 0.25)
            {
                _Rigid.velocity = Vector3.zero;
                _Rigid.AddForce(Vector3.up * _JumpingPower, ForceMode.Impulse);

                SetState(PlayerState.DoudbleJumping);
                _IsDJump = true;
                _Anim.SetIsDJump(_IsDJump);
                _Eff.EffectPlay(Effect.DJump);
            }

        }



        void Move(int dir)      /*이동 및 방향전환*/
        {
            if (_IsAttack) return;
            _IsRunning = true;
            _RunTime += Time.deltaTime;
            if (!_IsJump)
            {
                if (dir == 0) return;
                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                this.transform.position += transform.forward * _MoveSpeed * Time.deltaTime;

                if (dir > 0)
                {
                    SetState(PlayerState.Running);
                    _Eff.EffectPlay(Effect.RRun);
                }
                else
                {
                    SetState(PlayerState.Running);
                    _Eff.EffectPlay(Effect.LRun);
                }
            }
            else
            {

                if (dir == 0)
                {
                    this.transform.position -= Vector3.up * _DownSpeed * Time.deltaTime;

                }
                else
                {
                    this.transform.position += Vector3.right * dir * _MoveSpeed / 2.0f * Time.deltaTime;
                }
            }

        }




        void Breaking()
        {
            //달리다가 멈추면 미끄러짐
            if (_IsOnesec && !_IsJump)//탄성 효과
            {
                if (_BreakTime > 0)
                {
                    //Debug.Log("이동");
                    _Rigid.AddForce(this.transform.forward * _SlideSpeed);
                    _BreakTime -= Time.deltaTime;
                }
                else
                {

                    _RunTime = 0.0f;
                    _IsOnesec = false;
                    if (dir > 0)
                        _Eff.EffectPlay(Effect.RBreak);
                    else
                        _Eff.EffectPlay(Effect.LBreak);
                }

            }
            _PrePos = transform.position;
        }


        void SetState(PlayerState t)
        {
            _State = t;
            _Anim.SetAnim(_State);
        }

        void Attack()
        {

            if (_IsKeyDown)//아래키를 누르고
            {
                if (_IsJump)//점프상태일때
                {
                    //아래공격
                }
                else//점프상태가 아닐 때
                {
                    //아래공격
                    SetState(PlayerState.DwAttack);
                }
            }
            else if (_IsKeyUp) //위키를 누른상태
            {
                //위공격

                SetState(PlayerState.UpAttack);//1.67


            }
            else //아래, 위키입력 없는 상태
            {

                if (_IsJump)//점프 상태
                {
                    if (_IsRunning)//달리는 상태
                    {

                        //전진공격
                        _IsAirAtk = true;
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                        SetState(PlayerState.RLAttack);
                    }
                    else
                    {

                        Debug.Log("점프중립공격 1");
                        //중립공격
                        _IsAirAtk = true;//이동이 멈춤
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                      
                        if (_State == PlayerState.NormalAttack)
                        {
                            SetState(PlayerState.NormalAttack2);
                            Debug.Log("normal2");
                        }
                        else if (_State == PlayerState.NormalAttack2)

                            SetState(PlayerState.NormalAttack3);
                        if (!_IsAttack)
                            SetState(PlayerState.NormalAttack);


                    }
                }
                else
                {
                    if (_IsRunning)//달리는 상태
                    {


                        if (_IsOnesec)//1초지난상태
                        {
                            //질주공격-
                        }
                        else
                        {
                            //전진공격
                            SetState(PlayerState.RLAttack);

                            Debug.Log("전진공격");
                        }
                    }
                    else
                    {
                        //중립공격
                        Debug.Log("중립공격 1");


                        if (_State == PlayerState.NormalAttack)
                        {
                            SetState(PlayerState.NormalAttack2);
                            Debug.Log("normal2");
                        }
                        else if (_State == PlayerState.NormalAttack2)

                            SetState(PlayerState.NormalAttack3);
                        if (!_IsAttack)
                            SetState(PlayerState.NormalAttack);



                    }
                }
            }


            //달리기 1초전공격(동일)
            //위공격(동일)



            if (!_IsAttack)
            {

                _Anim.TriggerAtk();
                _IsAttack = true;
                _Anim.SetIsAttack(_IsAttack);

            }
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
                if (_State != PlayerState.Idle && _JumpTime > _JumpRestriction)
                {
                    if (_IsJump)
                        _Eff.EffectPlay(Effect.Land);
                    _Rigid.velocity = Vector3.zero;
                    _Floortime = 0.0f;
                    if (!_IsAttack)
                    {
                        _State = PlayerState.Idle;
                        _Anim.SetAnim(_State);

                    }
                    _IsRunning = false;
                    _IsJump = false;
                    _IsDJump = false;
                    _Anim.SetIsJump(_IsJump);
                    _Anim.SetIsDJump(_IsDJump);



                }

            }
        }




        public void onRLMove()
        {
            _IsRLMove = true;
            _RLMovePos = this.transform.position;

            Debug.Log("이거 1번만 실행됨");
        }

        public void setinit()//초기화
        {
            
                _IsAttack = false;
                _Anim.SetIsAttack(_IsAttack);
                _IsRLMove = false;
                _IsAirAtk = false;
                _Anim.TriggerReset();
                

        }


    }





}



