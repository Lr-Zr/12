using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine.Playables;
using System.IO.IsolatedStorage;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;

namespace nara
{

    public class PlayerCtrller : MonoBehaviour
    {


        //�̵��ӵ�
        [SerializeField]
        float _MoveSpeed = 10.0f;

        //������
        [SerializeField]
        float _JumpingPower = 10.0f;

        //������
        [SerializeField]
        float _Gauge = 1.0f;

        //���
        [SerializeField]
        int _Life = 3;

        //�̲������� �ӵ�
        [SerializeField]
        float _SlideSpeed = 50.0f;

        //�ϰ�Ű ������ ���ǵ�
        [SerializeField]
        float _DownSpeed = 3.0f;

        //���� ��  �ൿ����
        [SerializeField]
        float _JumpRestriction = 0.25f;

        //���� ���� �ð�
        [SerializeField]
        float _RunRestriction = 1.0f;


        //�����ð�
        [SerializeField]
        float _SlideTime = 0.25f;

        //�Ŀ�
        [SerializeField]
        public Vector3 _Power = Vector3.zero;

        [SerializeField]
        public float Dmg = 0.0f;

        //�������� ���ǵ�
        [SerializeField]
        float RLspeed = 1000f;


        Rigidbody _Rigid;
        PlayerAnimation _Anim;
        PlayerEffect _Eff;
        PlayerState _State;
        //���� �¿� move�Լ��� �� ����Ʈ ���� ��ȯ
        public int dir;
        //time
        public float _RunTime = 0.0f;
        float _JumpTime = 0.3f;
        float _Floortime = 0.0f;//



        public bool _IsAttack;
        bool _IsJump;
        bool _IsDJump;
        bool _IsOnesec;//���� ���� 
        bool _IsRunning;

        public bool _IsRLMove;//��������

        //keydown time;
        float _KDwTime = 0f;
        float _KUpTime = 0f;

        //keydown bool;
        bool _IsKeyDown;
        bool _IsKeyUp;

        //�Ÿ� ������ 
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
        }

        private void FixedUpdate()
        {
            Debug.Log("����" + dir);
            //�޸��ٰ� ���ߴ� ����
            _Floortime += Time.deltaTime;

            if (_IsRLMove)//�Ÿ��� ����̻�Ǹ� �����.
            {
                float x = Vector3.Distance(_PrePos, _RLMovePos);
                _Rigid.AddForce(this.transform.forward * RLspeed,ForceMode.Force);
                if(x>=3.0f)
                {
                    _IsRLMove = false;
                }
               // Debug.Log("���� �ȿ��� ������ ���� ��");
            }

            //Ŀ�ǵ� Ű�Է�
            _KUpTime -= Time.deltaTime;
            _KDwTime -= Time.deltaTime;
            if (_KUpTime < 0) _IsKeyUp = false;
            else _IsKeyUp = true;
            if (_KDwTime < 0) _IsKeyDown = false;
            else _IsKeyDown = true;
            _Anim.SetIsUpKey(_IsKeyUp);
            _Anim.SetIsDwKey(_IsKeyDown);

            //ü�� �ð�
            if (_IsJump)
                _JumpTime += Time.deltaTime;

            //�극��ŷ ���� ���� ���� RunTime;
            if (_RunTime > _RunRestriction && !_IsJump)
            {
                _RunTime = _RunRestriction;
                _IsOnesec = true;
            }
            //�޸����� �޸��� �ʴ��� �Ǵ�.

            if (_RunTime > 0) _IsRunning = true;
            else _IsRunning = false;



            _Anim.SetIsRunning(_IsRunning);
            if (_State != PlayerState.Running && !_IsAttack && !_IsOnesec)
            {
                if (!_IsJump)
                    _RunTime = 0.0f;
                else
                    _RunTime = 0.1f;
            }




            //���ϻ��� 
            if (_Rigid.velocity.y < -0.05f && _IsJump)//����
            {
                SetState(PlayerState.Falling);

            }


        }
        void Update()
        {
            //�ٴڿ� �ִ��� üũ �Լ�;
            OnFloor();
            //�극��ŷ �κ�
            Breaking();

        }
        private void LateUpdate()
        {
            //_Eff.EffectPlay(Effect.Test);
        }
        void OnKeyboard()
        {
            //if (!_Pv.IsMine) return;

            /* ���� �Է� */
            if (Input.GetKey(KeyCode.DownArrow))//���ձ� �� �� �ϰ� �ӵ� ���
            {

                _KUpTime = 0.0f;
                _KDwTime = 0.2f;
                if (_IsJump && _JumpTime > _JumpRestriction)
                    Move(0);
            }

            else if (Input.GetKey(KeyCode.UpArrow))//����Ű ��
            {
                _KUpTime = 0.2f;
                _KDwTime = 0.0f;


            }

            /* �̵� �Է� */
            if (Input.GetKey(KeyCode.LeftArrow))//�� �̵�
            {
                dir = -1;
                Move(dir);
            }
            if (Input.GetKey(KeyCode.RightArrow))//�� �̵�
            {
                dir = 1;
                Move(dir);
            }





            if (Input.GetKey(KeyCode.Q)) //����
            {
                Attack();
            }

            else if (Input.GetKey(KeyCode.W))//��ų
            {

            }

            else if (Input.GetKey(KeyCode.E))//���
            {

            }

            else if (Input.GetKey(KeyCode.R))//���
            {

            }



            /* ���� */
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



        void Move(int dir)      /*�̵� �� ������ȯ*/
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
            //�޸��ٰ� ���߸� �̲�����
            if (_IsOnesec && !_IsJump)//ź�� ȿ��
            {
                _RunTime -= Time.deltaTime;
                if (_RunTime > _RunRestriction - _SlideTime)
                {
                    //Debug.Log("�̵�");
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


        void SetState(PlayerState t)
        {
            _State = t;
            _Anim.SetAnim(_State);
        }

        void Attack()
        {
            float time = 0.0f;
            if (_IsKeyDown && !_IsAttack)//�Ʒ�Ű�� ������
            {
                if (_IsJump)//���������϶�
                {
                    //�Ʒ�����
                }
                else//�������°� �ƴ� ��
                {
                    //�Ʒ�����
                }
            }
            else if (_IsKeyUp && !_IsAttack) //��Ű�� ��������
            {
                //������

                SetState(PlayerState.UpAttack);//1.67


            }
            else//�Ʒ�, ��Ű�Է� ���� ����
            {
                if (_IsJump)//���� ����
                {
                    //�߸�����

                    //��������
                }
                else
                {
                    if (_IsRunning)//�޸��� ����
                    {
                        if (_IsOnesec)//1����������
                        {
                            //���ְ���
                            return;
                        }
                        else
                        {
                            //��������
                            SetState(PlayerState.RLAttack);

                            Debug.Log("��������");
                        }
                    }
                    else
                    {
                        //�߸�����
                    }
                }
            }


            //�޸��� 1��������(����)
            //������(����)



            if (!_IsAttack)
            {

                _Anim.TriggerAtk();
                _IsAttack = true;
                _Anim.SetIsAttack(_IsAttack);

            }
        }

        public void setisAttack()
        {

            _IsAttack = false;
            _Anim.SetIsAttack(_IsAttack);
            _IsRLMove = false;

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
            Debug.Log("�̰� 1���� �����");
        }


    }





}



