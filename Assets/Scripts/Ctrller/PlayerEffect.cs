using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace nara
{


    public class PlayerEffect : MonoBehaviour
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


        PlayerCtrller _playerCtrller;
        [SerializeField]
        GameObject _hit;
        GameObject _hitgo;
        void Start()
        {
            go = new GameObject[(int)Effect.End];
            atkgo = new GameObject[(int)AtkEffect1.End];
            SearchInChildren(this.transform);
            _playerCtrller = GetComponent<PlayerCtrller>();
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
            if (type != 22)
            {

                if (atkgo[type] != null) return;
            }

            _Pos = _TipOfSword.position;
            switch (type)
            {
                case 0://Up1
                    _Pos = this.transform.position + _AtkEffects[type].transform.position; //캐릭터위치 + 이펙트가 가지고 있는 포지션

                    break;
                case 1://Up2
                    break;
                case 2://RL1
                    _Pos = this.transform.position + _AtkEffects[type].transform.position; //캐릭터위치 + 이펙트가 가지고 있는 포지션
                    break;
                case 3://RL2
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                    }

                    break;
                case 4://Dw1
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 5://Dw2
                    break;
                case 6://기본공격1
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1); ;
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 7://기본공격2
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, -1, 1);
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(-90f, 0, 0);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, -1, 1); ;
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(90f, 0, 0);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 8://기본공격3
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                        _AtkEffects[type].transform.localPosition = new Vector3(1f, 0.6f, 0f);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                        _AtkEffects[type].transform.localPosition = new Vector3(-1f, 0.6f, 0f);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 9:
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;

                    break;
                case 10://공중아래공격
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
                case 11://공중기본공격
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1);
                    }
                    _Pos += _AtkEffects[type].transform.position;
                    break;
                case 12://위스킬1
                    break;
                case 13://위스킬2
                    break;
                case 14://위스킬3
                    break;
                case 15://RL스킬1
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {

                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 180f, 0);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 16://RL스킬2
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {

                        _AtkEffects[type].transform.localPosition = new Vector3(-4f, 0.6f, 0f);
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {

                        _AtkEffects[type].transform.localPosition = new Vector3(4f, 0.6f, 0f);
                        _AtkEffects[type].transform.localEulerAngles = new Vector3(0, 180f, 0);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 17://Dw스킬1
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 18://Dw스킬2
                    break;
                case 19://노말스킬1

                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 20://노말스킬2
                    if (_playerCtrller.dir > 0.0f)//이펙트 방향 변환
                    {

                        _AtkEffects[type].transform.localEulerAngles = new Vector3(90f, 90f, 0);
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {

                        _AtkEffects[type].transform.localEulerAngles = new Vector3(-90f, 90f, 0);
                    }
                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    break;
                case 21://방어
                    _Pos = this.transform.position;
                    break;
                case 22://넉아웃
                    _Pos = this.transform.position;
                    break;


            }

            atkgo[type] = Instantiate(_AtkEffects[type], _Pos, _AtkEffects[type].transform.rotation);
        }
        public void EffectOff(int type)
        {
            Destroy(atkgo[type]);
        }
        public void EffDestroyWithTime(int type,float time)
        {
            Destroy(atkgo[type],time);
        }
        void SearchInChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // 조건을 만족하는지 여부를 검사
                if (child.name == "TipofSword")
                {
                    _TipOfSword = child;

                }

                // 자식 오브젝트의 자식들을 검색하기 위해 재귀 호출
                SearchInChildren(child);
            }
        }

        private void Update()
        {
            if (atkgo[10] != null)
                atkgo[10].transform.position = this.transform.position + _AtkEffects[10].transform.position; ;
            if (atkgo[11] != null)
                atkgo[11].transform.position = _TipOfSword.position + _AtkEffects[11].transform.position; ;

            if (atkgo[17] != null)
                atkgo[17].transform.position = this.transform.position + _AtkEffects[17].transform.position; ;
        }


        public void Hitted(Vector3 pos, float time)
        {

            _hitgo = Instantiate(_hit, pos, _hit.transform.rotation);
            Destroy(_hitgo, time);
        }
    }

}