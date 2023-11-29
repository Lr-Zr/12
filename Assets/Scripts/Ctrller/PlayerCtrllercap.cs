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

    public class PlayerCtrllercap : MonoBehaviour
    {


        [SerializeField]
        string _Name = "Cap";

        [SerializeField]
        public int playertype = 2;

        //이동속도
        [SerializeField]
        float _MoveSpeed = 10.0f;

        //점프력
        [SerializeField]
        float _JumpingPower = 13.0f;

        //게이지
        [SerializeField]
        public float _Gauge = 1.0f;

        //목숨
        [SerializeField]
        public int _Life = 3;

        //미끄러지는 속도
        [SerializeField]
        float _SlideSpeed = 20.0f;

        //하강키 누를때 스피드
        [SerializeField]
        float _DownSpeed = 5.0f;

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

        [SerializeField]
        float Upspeed = 20f;


        Rigidbody _Rigid;
        PlayerAnimation _Anim;
        PlayerEffectcap _Eff;
        PlayerColliderscap _Cols;
        GameObject _objgauge;

        public PlayerState _State;
        //방향 좌우 move함수용 및 이펙트 방향 변환
        public int dir;
        public int airdir;
        //time
        public float _RunTime = 0.0f;
        float _BreakTime = 0.0f;
        float _JumpTime = 0.3f;
        float _Floortime = 0.0f;//
        float _GodTime = 0.0f;

        //공격 지연 시간 및 조건
        float _AttackTime = 0.0f;
        bool _CantAttack;//

        public bool _IsAttack;
        public bool _IsSkill;
        bool _IsJump;
        bool _IsDJump;
        bool _IsOnesec;//질주 공격 
        public bool _IsRunning;
        bool _IsKnockOut;
        bool _IsShield;
        public float _ECooldown = 0f;//방어키 딜레이



        float _Teemo;

        /* 공격 스킬 관련 변수 */
        bool _IsRLMove;//전진공격 이동시작
        bool _IsUpMove;//위스킬 이동시작
        bool _IsAirAtk;//중력0으로
        //keydown time;
        float _KDwTime = 0f;
        float _KUpTime = 0f;
        float _KQTime = 0f;//공격키 딜레이
        float _KWTime = 0f;//스킬키 딜레이

        //keydown bool;
        bool _IsKeyDown;
        bool _IsKeyUp;
        bool _IsKeyQ;
        bool _IskeyE;
        //거리 측정용 
        Vector3 _MovePos;
        Vector3 _PrePos;

        float stuntime = 0.1f;//경직시간
        float outtime;//넉아웃 시간//
        float startime;//넉아웃 이펙트 생성시간;
      

        //test
        [SerializeField]
        float kt1;
        [SerializeField]
        float kt2;
        [SerializeField]
        float kt3;
        [SerializeField]
        float kt4;
        [SerializeField]
        float kt5;

        Vector3 hittedPower;

        void Start()
        {

            _Rigid = GetComponent<Rigidbody>();
            _Anim = GetComponent<PlayerAnimation>();
            _Eff = GetComponent<PlayerEffectcap>();
            _Cols = GetComponent<PlayerColliderscap>();
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
            _IsUpMove = false;
            _IsAirAtk = false;
            playertype = 2;


            TypeOfPlayer(2);
        }

        private void FixedUpdate()
        {
            //방어키 쿨타임 
            _ECooldown -= Time.deltaTime;

            if (_IsShield)
            {
                if (_ECooldown < 5.0f)
                {
                    Debug.Log("test1");
                    _IsShield = false;

                    _Eff.EffectOff(15);
                }

            }
            //넉아웃 상태에서 이동
            if (_IsKnockOut) 
            {
                startime += Time.deltaTime;
                if(startime>0.05f)
                {
                    //이펙트 생성
                    _Eff.EffectOn(16);
                    _Eff.EffDestroyWithTime(16, 1);
                    startime = 0;
                }
                if (outtime < 0)
                {
                    _IsKnockOut = false;
                    _Rigid.velocity = Vector3.zero;
                    startime = 0;
                  
                }
                KnockOutMove();

                outtime -= Time.deltaTime;
            }
            stuntime -= Time.deltaTime;


            //달리다가 멈추는 조건
            _Floortime += Time.deltaTime;
            //공격 지연
            if (_CantAttack)
            {
                _AttackTime -= Time.deltaTime;
                if (_AttackTime < 0f)
                {
                    _IsAttack = false;
                    _CantAttack = false;
                    _IsSkill = false;
                    _Anim.SetIsAttack(_IsAttack);
                    _Anim.SetIsSkill(_IsSkill);
                }
            }



            if (_IsAirAtk)
                _Rigid.velocity = Vector3.zero;

            if (_IsRLMove)//거리가 어느이상되면 멈춘다.
            {
                float x = Vector3.Distance(_PrePos, _MovePos);
                if (x < _Teemo)
                {
                    _Rigid.AddForce(this.transform.forward * RLspeed, ForceMode.Force);
                    Debug.Log("cap " + RLspeed);
                }
            }
            if (_IsUpMove)
            {
                _IsAirAtk = false;
                _Rigid.AddForce(this.transform.up * Upspeed, ForceMode.Impulse);
                _IsUpMove = false;
            }


            //커맨드 키입력
            _KUpTime -= Time.deltaTime;
            _KDwTime -= Time.deltaTime;
            _KQTime -= Time.deltaTime;
            _KWTime -= Time.deltaTime;
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
                _Anim.SetIsOnesec(_IsOnesec);
                _BreakTime = _BreakingTime;
            }
            //달리는지 달리지 않는지 판단.

            if (_RunTime > 0) _IsRunning = true;
            else _IsRunning = false;
            _Anim.SetIsRunning(_IsRunning);


            //Runtime 초기화;
            if (_State != PlayerState.Running && !_IsAttack && !_IsOnesec && !_IsSkill)
            {
                _RunTime = 0.0f;
            }


            //낙하상태 
            if (_Rigid.velocity.y < -0.05f && !_IsAttack && !_IsSkill && stuntime < 0f&&!_IsKnockOut)//낙하
            {
                SetState(PlayerState.Falling);

            }

        }
        void Update()
        {
            if (_objgauge)
            {
                _objgauge.GetComponent<PlayerGauge>()._gauge = _Gauge;
                _objgauge.transform.position = this.transform.position;

            }

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

            if ((_IsKnockOut || _IsShield) || stuntime >= 0f) return;
            if (playertype == 1)
            {
                if (Input.GetKey(KeyCode.S))//조합기 하 및 하강 속도 향상
                {

                    _KUpTime = 0.0f;
                    _KDwTime = 0.2f;
                    if (_IsJump && _JumpTime > _JumpRestriction && !_IsAttack && !_IsSkill)
                        Move(0);
                }

                else if (Input.GetKey(KeyCode.W))//조합키 상
                {
                    _KUpTime = 0.2f;
                    _KDwTime = 0.0f;


                }

                /* 이동 입력 */
                if (Input.GetKey(KeyCode.A))//좌 이동
                {
                    if (!_IsAttack && !_IsSkill)
                    {
                        dir = -1;
                        Move(dir);
                    }

                }
                if (Input.GetKey(KeyCode.D))//우 이동
                {
                    if (!_IsAttack && !_IsSkill)
                    {
                        dir = 1;
                        Move(dir);

                    }

                }


                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (_KQTime < 0.0f && !_IsSkill)
                    {
                        _KQTime = 0.3f;
                        Attack();

                    }
                }
                //else if (Input.GetKey(KeyCode.Q)) //공격
                //{

                //}

                else if (Input.GetKeyDown(KeyCode.G))//스킬
                {
                    if (_KWTime < 0.0f && !_IsAttack && !_IsSkill)
                    {
                        _KWTime = 0.3f;
                        Skill();
                    }
                }

                else if (Input.GetKey(KeyCode.H))//방어
                {
                    if ((_State == PlayerState.Idle || _State == PlayerState.Running) && _ECooldown < 0f)
                    {
                        SetState(PlayerState.Sheild);
                        _IsShield = true;
                        _ECooldown = 6.0f;
                        _Anim.TriggerShield();

                    }
                }

                /* 점프 */
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();

                }
            }
            else
            {
                if (Input.GetKey(KeyCode.DownArrow))//조합기 하 및 하강 속도 향상
                {

                    _KUpTime = 0.0f;
                    _KDwTime = 0.2f;
                    if (_IsJump && _JumpTime > _JumpRestriction && !_IsAttack && !_IsSkill)
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
                    if (!_IsAttack && !_IsSkill)
                    {
                        dir = -1;
                        Move(dir);
                    }

                }
                if (Input.GetKey(KeyCode.RightArrow))//우 이동
                {
                    if (!_IsAttack && !_IsSkill)
                    {
                        dir = 1;
                        Move(dir);

                    }

                }


                if (Input.GetKeyDown(KeyCode.L))
                {
                    if (_KQTime < 0.0f && !_IsSkill)
                    {
                        _KQTime = 0.3f;
                        Attack();

                    }
                }
                //else if (Input.GetKey(KeyCode.Q)) //공격
                //{

                //}

                else if (Input.GetKeyDown(KeyCode.Semicolon))//스킬
                {
                    if (_KWTime < 0.0f && !_IsAttack && !_IsSkill)
                    {
                        _KWTime = 0.3f;
                        Skill();
                    }
                }

                else if (Input.GetKey(KeyCode.Quote))//방어
                {
                    if ((_State == PlayerState.Idle || _State == PlayerState.Running) && _ECooldown < 0f)
                    {
                        SetState(PlayerState.Sheild);
                        _IsShield = true;
                        _ECooldown = 6.0f;
                        _Anim.TriggerShield();

                    }
                }

                /* 점프 */
                if (Input.GetKey(KeyCode.RightShift))
                {
                    Jump();

                }
            }
            //1p//////////////////////////////////////////////////////////////////////////////
            /* 상하 입력 */

            //1p//////////////////////////////////////////////////////////////////////////////
        }




        void Jump()
        {

            if (_IsAttack || _IsSkill) return;
            _RunTime = 0;
            _IsOnesec = false;

            _Anim.SetIsOnesec(_IsOnesec);

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
            _IsRunning = true;
            _Anim.SetIsRunning(_IsRunning);
            _RunTime += Time.deltaTime;
            if (!_IsJump)
            {

                if (dir == 0) return;
                SetState(PlayerState.Running);

                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                this.transform.position += transform.forward * _MoveSpeed * Time.deltaTime;


                if (dir > 0)
                {

                    _Eff.EffectPlay(Effect.RRun);
                }
                else
                {
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

                    _Anim.SetIsOnesec(_IsOnesec);
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
            _State = t;//코드내의 상태
            _Anim.SetAnim(_State);//애니메이터의상태
        }

        void Attack()
        {

            if (_IsKeyDown)//아래키를 누르고
            {
                if (_IsJump)//점프상태일때
                {
                    //공중아래공격 
                    if (!_IsAttack)
                    {
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                        SetState(PlayerState.AirDwAttack);

                    }
                }
                else//점프상태가 아닐 때
                {
                    //아래공격 X

                    SetState(PlayerState.DwAttack);
                }
            }
            else if (_IsKeyUp) //위키를 누른상태
            {
                //위공격
                _IsAirAtk = true;//이동이 멈춤
                SetState(PlayerState.UpAttack);//1.67


            }
            else //아래, 위키입력 없는 상태
            {

                if (_IsJump)//점프 상태
                {
                    if (_IsRunning)//달리는 상태
                    {

                        //공중전진공격
                        _Teemo = 3.0f;
                        _IsAirAtk = true;

                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                        SetState(PlayerState.RLAttack);

                    }
                    else
                    {
                        //공중중립공격


                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                        SetState(PlayerState.AirNormalAttack);




                    }
                }
                else//땅에 있는 상태
                {
                    if (_IsRunning)//달리는 상태
                    {


                        if (_IsOnesec)//1초지난상태
                        {
                            //땅질주공격-
                            _Teemo = 2.0f;
                            SetState(PlayerState.RunAttack);
                        }
                        else
                        {
                            //땅전진공격
                            SetState(PlayerState.RLAttack);
                            _Teemo = 3.0f;
                            //Debug.Log("전진공격");
                        }
                    }
                    else
                    {
                        //땅중립공격

                        if (!_IsKeyQ && !_CantAttack)//Q중립공격중 q키가 눌렸을 때
                        {
                            if (_State == PlayerState.NormalAttack)
                            {
                                SetState(PlayerState.NormalAttack2);

                                _IsKeyQ = true;
                            }
                            else if (_State == PlayerState.NormalAttack2)
                            {
                                SetState(PlayerState.NormalAttack3);
                                _IsKeyQ = true;
                            }
                        }
                        if (!_IsAttack)
                            SetState(PlayerState.NormalAttack);



                    }
                }
            }

            if (!_IsAttack)
            {

                _Anim.TriggerAtk();
                _IsAttack = true;
                _Anim.SetIsAttack(_IsAttack);

            }
        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        void Skill()
        {

            if (_IsKeyDown)//아래키를 누르고
            {
                if (_IsJump)//점프상태일때
                {
                    //아래공격
                    this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);

                    _IsAirAtk = true;//이동이 멈춤
                    SetState(PlayerState.DwSkill);

                }
                else//점프상태가 아닐 때
                {
                    SetState(PlayerState.DwSkill);

                }
            }
            else if (_IsKeyUp) //위키를 누른상태
            {
                //위공격
                _IsAirAtk = true;
                _IsJump = true;
                _Anim.SetIsJump(_IsJump);
                SetState(PlayerState.UpSkill);

            }
            else //아래, 위키입력 없는 상태
            {

                if (_IsJump)//점프 상태
                {
                    if (_IsRunning)//달리는 상태
                    {
                        //공중전진공격
                        _Teemo = 10f;
                        _IsAirAtk = true;//이동이 멈춤
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);

                        SetState(PlayerState.RLSkill);
                    }
                    else
                    {
                        //공중 중립공격
                        _IsAirAtk = true;//이동이 멈춤
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);
                        SetState(PlayerState.NormalSkill);

                    }
                }
                else//땅에 있는 상태
                {
                    if (_IsRunning)//달리는 상태
                    {
                        _Teemo = 10f;
                        //땅전진공격
                        SetState(PlayerState.RLSkill);
                    }
                    else
                    {
                        //땅중립공격
                        SetState(PlayerState.NormalSkill);

                    }
                }

            }


            if (!_IsSkill)
            {
                _Anim.TriggerSkill();
                _IsSkill = true;
                _Anim.SetIsSkill(_IsSkill);

            }
        }


        void OnFloor()
        {
            RaycastHit hit;
            Vector3 StartPos = transform.position;
            StartPos.y += 0.1f;
            Debug.DrawRay(StartPos, this.transform.up * -0.15f, Color.green);
            LayerMask mask = LayerMask.GetMask("Floor");
            if (Physics.Raycast(StartPos, this.transform.up * -1, out hit, 0.15f, mask))
            {
                //&& _Floortime > 0.05
                if (_State != PlayerState.Idle && _JumpTime > _JumpRestriction)
                {
                    if (_IsJump)
                        _Eff.EffectPlay(Effect.Land);
                    _Rigid.velocity = Vector3.zero;
                    _Floortime = 0.0f;
                    if (!_IsAttack && !_IsSkill && !_IsShield && stuntime < 0f && !_IsKnockOut)
                    {
                        _State = PlayerState.Idle;
                        _Anim.SetAnim(_State);

                        _IsJump = false;
                        _Anim.SetIsJump(_IsJump);
                    }
                    _IsRunning = false;
                    _Anim.SetIsRunning(_IsRunning);
                    _IsDJump = false;
                    _Anim.SetIsDJump(_IsDJump);


                    _IsAirAtk = false;

                }

            }
        }




        public void onRLMove()
        {
            _IsRLMove = true;
            _MovePos = this.transform.position;
        }
        public void onUpMove()
        {
            _IsUpMove = true;
        }

        public void setinit()//초기화
        {
            if (_IsKeyQ)
            {
                _IsKeyQ = false;

            }
            else
            {
                _Rigid.velocity = Vector3.zero;
                _CantAttack = true;
                _AttackTime = 0.05f;

                //    _IsAttack = false;
                _IsRLMove = false;
                _IsUpMove = false;
                _IsAirAtk = false;

                _Anim.TriggerReset();
            }


        }



        private void OnTriggerEnter(Collider other)
        {



            //방어스킬 쓸때는 하지말것.
            if (playertype == 1)
            {
                if (other.gameObject.tag == "2PA")
                {

                    PlayerCtrller pc = other.transform.root.GetComponent<PlayerCtrller>();
                    this.dir = -pc.dir;
                    pc.HitRot();

                    if (pc._Power.x == 0 && pc._Power.y == 0) //경직
                    {
                        stuntime = 0.2f;
                        _Gauge += pc.Dmg;
                        SetState(PlayerState.Stun);
                        _Anim.TriggerStun();
                    }
                    else//넉아웃
                    {

                        setinit();
                        _Gauge += pc.Dmg;
                        hittedPower = pc._Power * _Gauge;
                        _IsKnockOut = true;
                        setOutTime();
                        SetState(PlayerState.KnockOut);
                        _Anim.TriggerKnockOut();
                    }



                    _Eff.Hitted(other.transform.position, 1);

                }

            }
            else
            {
                if (other.gameObject.tag == "1PA")
                {


                    PlayerCtrller pc = other.transform.root.GetComponent<PlayerCtrller>();
                    dir = -pc.dir;
                    pc.HitRot();
                    if (pc._Power.x == 0 && pc._Power.y == 0) //경직
                    {
                        stuntime = 0.2f;
                        _Gauge += pc.Dmg;
                        SetState(PlayerState.Stun);
                        _Anim.TriggerStun();
                    }
                    else//넉아웃
                    {
                        setinit();
                        _Gauge += pc.Dmg;
                        hittedPower = pc._Power * _Gauge;
                        _IsKnockOut = true;

                        setOutTime();
                        SetState(PlayerState.KnockOut);
                        _Anim.TriggerKnockOut();
                        
                    }

                    _Eff.Hitted(other.transform.position, 1);
                }
            }
        }

        public void HitRot()
        {

            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * dir), 1f);

        }

        public void TypeOfPlayer(int type)
        {
            if (type == 1)
            {
                dir = 1;

                this.transform.eulerAngles = new Vector3(0, -90f, 0);
                //위치 넣기

                //태그설정
                this.tag = "1P";
                _Cols.SetTag("1PA");

                //게이지 찾기
                _objgauge = GameObject.Find("1pGauge");
                _objgauge.transform.position = this.transform.position;
                _objgauge.GetComponent<PlayerGauge>()._pType = playertype;
                _objgauge.GetComponent<PlayerGauge>()._gauge = _Gauge;



            }
            else
            {
                dir = -1;
                this.transform.eulerAngles = new Vector3(0, -90f, 0);
                //위치 넣기
                this.tag = "2P";

                _Cols.SetTag("2PA");
                _objgauge = GameObject.Find("2pGauge");
                _objgauge.transform.position = this.transform.position;

                _objgauge.GetComponent<PlayerGauge>()._pType = playertype;
                _objgauge.GetComponent<PlayerGauge>()._gauge = _Gauge;

            }
        }
        void setOutTime()
        {
            if (_Gauge <= 25f) outtime = kt1;
            else if (_Gauge <= 50f) outtime = kt2;
            else if (_Gauge <= 90f) outtime = kt3;
            else if (_Gauge <= 150f) outtime = kt4;
            else outtime = kt5;
            //if (_Gauge <= 25f) outtime = 0.1f;
            //else if (_Gauge <= 50f) outtime = 0.2f;
            //else if (_Gauge <= 90f) outtime = 0.3f;
            //else if (_Gauge <= 150f) outtime = 0.4f;
            //else outtime = 0.5f;
        }


        void KnockOutMove()
        {
             // 알아서 찾으셈 
            //Vector3 v = new Vector3(-dir*hittedPower.x, hittedPower.y, 0);
            //_Rigid.AddForce(v * 0.01f, ForceMode.Force);


            Vector3 v = new Vector3(-dir * hittedPower.x*30, hittedPower.y*5, 0);
            _Rigid.AddForce(v * 0.01f, ForceMode.Force);


        }
    }





}



