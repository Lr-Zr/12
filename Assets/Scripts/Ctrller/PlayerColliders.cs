using nara;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nara
{

    public class PlayerColliders : MonoBehaviour
    {

        [SerializeField]
        public GameObject[] _Collider = new GameObject[11];

        PlayerCtrller _playerCtrller;
        void Start()
        {
            for (int i = 0; i < _Collider.Length; i++)
            {
                if (_Collider[i] != null)
                    _Collider[i].SetActive(false);
            }
            _playerCtrller = this.GetComponent<PlayerCtrller>();

        }


       
        /* 공격으로 충돌오브젝트 온오프*/
        public void ActiveOn(int atk)
        {
            switch (atk)
            {
                case 0://Up
                    _playerCtrller._Power = new Vector3(0, 20, 0);
                    _playerCtrller.Dmg = 7;
                    break;
                case 1://RL
                    _playerCtrller._Power = new Vector3(20, 10, 0);
                    _playerCtrller.Dmg = 12;
                    break;
                case 2://Dw
                    _playerCtrller._Power = new Vector3(10, 10, 0);
                    _playerCtrller.Dmg = 5;
                    break;
                case 3://atk1,2
                    _playerCtrller._Power = new Vector3(0, 0, 0);
                    _playerCtrller.Dmg = 1;
                    break;
                case 4://atk3
                    _playerCtrller._Power = new Vector3(10, 20, 0);
                    _playerCtrller.Dmg = 5;
                    break;
                case 5://runatk
                    _playerCtrller._Power = new Vector3(10, 20, 0);
                    _playerCtrller.Dmg = 8;
                    break;
                case 6://airdownattack
                    _playerCtrller._Power = new Vector3(0, -5, 0);
                    _playerCtrller.Dmg = 7;
                    //hit수 3
                    break;
                case 7://airnormalattack
                    _playerCtrller._Power = new Vector3(20, 5, 0);
                    _playerCtrller.Dmg = 10;
                    
                    break;
                case 8://upskill
                    _playerCtrller._Power = new Vector3(0, 20, 0);
                    _playerCtrller.Dmg = 10;

                    break;
                case 9://RLSkill
                    _playerCtrller._Power = new Vector3(40, 20, 0);
                    _playerCtrller.Dmg = 20;
                    break;
                case 10://DwSKill
                    _playerCtrller._Power = new Vector3(0, -30, 0);
                    _playerCtrller.Dmg = 15;
                    break;
                case 11://NormalSkill
                    _playerCtrller._Power = new Vector3(20, 10, 0);
                    _playerCtrller.Dmg = 15;
                    break;
                case 12:
                    
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