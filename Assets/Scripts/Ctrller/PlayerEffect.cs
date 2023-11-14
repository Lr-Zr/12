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

        //�� �� 
        Transform _TipOfSword;

        Vector3 _Pos;



        PlayerCtrller _playerCtrller;
        float _EffAliveTime;
        void Start()
        {
            _EffAliveTime = 0.5f;
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

        //���� ���� ����Ʈ
        public void onEffects(int type)
        {
            //������ ���ݾ� �׷� �����ð� ����Ʈ
            Debug.Log("�����糪?");
            //Debug.Log(_AtkPos);
            if (atkgo[type] != null) return;

            _Pos = _TipOfSword.position;
            switch (type)
            {
                case 0://up1
                    _Pos = this.transform.position + _AtkEffects[type].transform.position; //ĳ������ġ + ����Ʈ�� ������ �ִ� ������
                    _EffAliveTime = 0.1f;

                    break;
                case 1://up2
                    _EffAliveTime = 0.9f;
                    break;
                case 2://RL1
                    _EffAliveTime = 0.1f;
                    break;
                case 3://RL2
                    if (_playerCtrller.dir > 0.0f)//����Ʈ ���� ��ȯ
                    {
                        _AtkEffects[type].transform.localScale = new Vector3(1, 1, 1);
                      
                    }
                    else if (_playerCtrller.dir < 0.0f)
                    {
                      
                        _AtkEffects[type].transform.localScale = new Vector3(-1, 1, 1); ;
                    }

                    _EffAliveTime = 0.4f;
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:

                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;

            }

            atkgo[type] = Instantiate(_AtkEffects[type], _Pos, Quaternion.identity);
            Destroy(atkgo[type], _EffAliveTime);
            Debug.Log("����Ʈ onEffects");
        }


        void SearchInChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // ������ �����ϴ��� ���θ� �˻�
                if (child.name == "TipofSword")
                {
                    _TipOfSword = child;

                }

                // �ڽ� ������Ʈ�� �ڽĵ��� �˻��ϱ� ���� ��� ȣ��
                SearchInChildren(child);
            }
        }

    }

}