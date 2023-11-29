using nara;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nara
{

    public class PlayerColliderscap : MonoBehaviour
    {

        [SerializeField]
        public GameObject[] _Collider = new GameObject[11];

        PlayerCtrllercap _playerCtrller;
        void Start()
        {
            for (int i = 0; i < _Collider.Length; i++)
            {
                if (_Collider[i] != null)
                    _Collider[i].SetActive(false);
            }
            _playerCtrller = this.GetComponent<PlayerCtrllercap>();

        }


       
        /* 공격으로 충돌오브젝트 온오프*/
        public void ActiveOn(int atk)
        {
            switch (atk)
            {
                case 0://노말공격1,2
                    _playerCtrller.Dmg = 3;
                    _playerCtrller._Power = new Vector3(0, 0, 0);
                    break;
                case 1://노말공격3
                    _playerCtrller._Power = new Vector3(40, 30, 0);
                    _playerCtrller.Dmg = 6;
                    break;



                case 2://위공격
                    _playerCtrller._Power = new Vector3(0, 40, 0);
                    _playerCtrller.Dmg = 8;
                    break;

                    
                case 3://아래공격
                    _playerCtrller._Power = new Vector3(40, 10, 0);
                    _playerCtrller.Dmg = 6;
                    break;



                case 4://좌우공격
                    _playerCtrller._Power = new Vector3(40, 20, 0);
                    _playerCtrller.Dmg = 15;
                    break;



                case 5://질주공격
                    _playerCtrller._Power = new Vector3(50, 10, 0);
                    _playerCtrller.Dmg = 13;
                    break;


                case 6://공중아래공격
                    _playerCtrller._Power = new Vector3(10, -30, 0);
                    _playerCtrller.Dmg = 12;
                    //hit수 3
                    break;
                //
                case 7://공중중립공격
                    _playerCtrller._Power = new Vector3(5, 40, 0);
                    _playerCtrller.Dmg = 4;
                    break;


                case 8://스킬 노말
                    _playerCtrller._Power = new Vector3(30, 20, 0);
                    _playerCtrller.Dmg = 10;

                    break;
                case 9://스킬 위
                    _playerCtrller._Power = new Vector3(30, 20, 0);
                    _playerCtrller.Dmg = 10;
                    break;


                case 10://스킬아래1
                    _playerCtrller._Power = new Vector3(0, 0, 0);
                    _playerCtrller.Dmg = 6;
                    break;
                case 11://스킬아래2
                    _playerCtrller._Power = new Vector3(0, 0, 0);
                    _playerCtrller.Dmg = 10;
                    break;
                case 12://스킬아래3
                    _playerCtrller._Power = new Vector3(50, -50, 0);
                    _playerCtrller.Dmg = 16;
                    break;
                case 13://스킬 좌우
                    _playerCtrller._Power = new Vector3(50, 20, 0);
                    _playerCtrller.Dmg = 13;
                    break;
            }
            
                    _Collider[atk].SetActive(true);
        }
        public void ActiveOff(int atk)
        {
            _Collider[atk].SetActive(false);
       
        }

        public void SetTag(string tag)
        {
            for (int i = 0; i < _Collider.Length; i++)
            {
                if (_Collider[i] != null)
                    _Collider[i].tag = tag;

            }
        }
    }

}