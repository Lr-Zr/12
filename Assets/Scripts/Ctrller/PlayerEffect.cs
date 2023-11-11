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
        Transform _AtkPos;

        Vector3 _Pos;
        float _EffAliveTime;
        void Start()
        {
            _EffAliveTime = 0.5f;
            go = new GameObject[(int)Effect.End];
            atkgo = new GameObject[(int)AtkEffect1.End];
            SearchInChildren(this.transform);
        }

        public void EffectPlay(Effect effect, float time = 1.0f)
        {
            if (go[(int)effect] != null) return;
            _Pos = this.transform.position + _Effects[(int)effect].transform.position;

            go[(int)effect] = Instantiate(_Effects[(int)effect], _Pos, Quaternion.identity);
            Destroy(go[(int)effect], time);

        }


        public void AtkEvent(int type)
        {
           //공격을 하잖아 그럼 생존시간 이펙트
            Debug.Log("실행됬나?");
            //Debug.Log(_AtkPos);
            if (atkgo[type] != null) return;
            switch (type)
            {
                case 0:
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

                    _Pos = this.transform.position + _AtkEffects[type].transform.position;
                    atkgo[type] = Instantiate(_AtkEffects[type], _Pos, Quaternion.identity);
                    Destroy(atkgo[type], _EffAliveTime);
                    return;
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;

            }
            atkgo[type] = Instantiate(_AtkEffects[type], _AtkPos.position, Quaternion.identity);
            Destroy(atkgo[type], _EffAliveTime);
            Debug.Log("끝났나?");
        }


        void SearchInChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // 조건을 만족하는지 여부를 검사
                if (child.name=="EffectPos")
                {
                    _AtkPos = child;
                }

                // 자식 오브젝트의 자식들을 검색하기 위해 재귀 호출
                SearchInChildren(child);
            }
        }

    }

}