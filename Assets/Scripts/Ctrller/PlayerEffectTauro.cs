using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace nara
{


    public class PlayerEffectTauro : MonoBehaviour
    {

        [SerializeField]
        GameObject[] _Effects;
        GameObject[] go;



        [SerializeField]
        GameObject[] _AtkEffects;
        GameObject[] atkgo;

        //검 끝 
        Transform _TipOfSword;

        Vector3 _Pos;
        Vector3 _Rot;
        Vector3 _original;

        PlayerCtrllerTauro _playerCtrller;

        void Start()
        {
            go = new GameObject[(int)Effect.End];
            atkgo = new GameObject[(int)AtkEffect1.End];
            SearchInChildren(this.transform);
            _playerCtrller = GetComponent<PlayerCtrllerTauro>();
        }

        public void EffectPlay(Effect effect, float time = 0.5f)
        {
            if (go[(int)effect] != null) return;
            _Pos = this.transform.position + _Effects[(int)effect].transform.position;

            go[(int)effect] = Instantiate(_Effects[(int)effect], _Pos, Quaternion.identity);
            Destroy(go[(int)effect], time);

        }

        //공격 관련 이펙트
        public void EffectOn(int type)
        {
            //공격을 하잖아 그럼 생존시간 이펙트

            //Debug.Log(_AtkPos);
            if (atkgo[type] != null) return;

            _Pos = _TipOfSword.position;
            switch (type)
            {
                case 0://기본공격1
                   
                        _AtkEffects[type].transform.localScale = new Vector3(_playerCtrller.dir, 1, 1);
                        
                   
                    _Pos += _AtkEffects[type].transform.position;
                    break;
                case 1://기본공격2
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);

                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, -45f);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, 45f);
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1); ;
                    }
                    _Pos += _AtkEffects[type].transform.position;
                    break;

                case 2://기본공격3
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);

                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1); ;

                    }
                    _Pos += _AtkEffects[type].transform.position;

                    break;


                case 3://위공격
                    break;

                case 4://아래공격

                    _AtkEffects[type].transform.position = new Vector3(_playerCtrller.dir, 0.3f, 0);

                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;




                case 5://좌우공격
                  
                    break;

                case 6://질주공격
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                    }
                    break;




                case 7://공중아래공격

                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, -1, 1);
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, 45f);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, -1, 1); ;
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, -45f);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;




                case 8://공중중립공격
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;



                case 9://스킬중립
                    _AtkEffects[type].transform.position = new Vector3(-7.5f * _playerCtrller.dir, -1.0f, 0f);

                    _AtkEffects[type].transform.localEulerAngles = new Vector3(0f, 90f * _playerCtrller.dir, 10f * _playerCtrller.dir);
                    _Pos += _AtkEffects[type].transform.position;
                    break;




                case 10://위스킬

                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;


                case 11://아래스킬1

                    _AtkEffects[type].transform.position = new Vector3(1 * _playerCtrller.dir, 0.6f, 0f);
                    _AtkEffects[type].transform.localEulerAngles = new Vector3(0f, -90 * _playerCtrller.dir, 0f);//좌우
                    _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;

                    break;


                case 12://아래스킬2

                    _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;

                    break;

                case 13://아래스킬3
                    _AtkEffects[type].transform.position = new Vector3(1.6f * _playerCtrller.dir, 0.7f, 0f);
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;


                case 14://아래스킬3
                    _AtkEffects[type].transform.position = new Vector3(-1 * _playerCtrller.dir, 0.7f, 0f);
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;

            }

            atkgo[type] = Instantiate(_AtkEffects[type], _Pos, _AtkEffects[type].transform.rotation);
            Debug.Log("이펙트 onEffects");
        }
        public void EffectOff(int type)
        {
            Destroy(atkgo[type]);
        }

        void SearchInChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // 조건을 만족하는지 여부를 검사
                if (child.name == "TipOfSword")
                {
                    _TipOfSword = child;

                }

                // 자식 오브젝트의 자식들을 검색하기 위해 재귀 호출
                SearchInChildren(child);
            }
        }

        private void Update()
        {
            if (atkgo[6] != null)
                atkgo[6].transform.position = this.transform.position + _AtkEffects[6].transform.position; ;
            if (atkgo[8] != null)
                atkgo[8].transform.position = this.transform.position + _AtkEffects[8].transform.position; ;
            if (atkgo[9] != null)
                atkgo[9].transform.position = _TipOfSword.position + _AtkEffects[9].transform.position; ;
            if (atkgo[10] != null)
                atkgo[10].transform.position = this.transform.position + _AtkEffects[10].transform.position; ;

        }
    }

}