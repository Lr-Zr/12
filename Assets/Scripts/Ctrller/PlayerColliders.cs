using nara;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nara
{

    public class PlayerColliders : MonoBehaviour
    {

        [SerializeField]
        GameObject[] _Collider = new GameObject[11];

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


        void Update()
        {

        }

        /* 공격으로 충돌오브젝트 온오프*/
        public void ActiveOn(int atk)
        {
            switch (atk)
            {
                case 0://Up
                    _playerCtrller._Power = new Vector3(0, 0, 0);
                    _Collider[atk].SetActive(true);
                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

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
        }
        public void ActiveOff(int atk)
        {
            _Collider[atk].SetActive(false);
            Debug.Log("왜 이거 안해?");
        }
    }

}