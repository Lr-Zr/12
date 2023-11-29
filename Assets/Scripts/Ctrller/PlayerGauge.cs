using nara;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGauge : MonoBehaviour
{

    public int _pType=-1;
    public float _gauge=-1;
    [SerializeField]
    public GameObject[] objs;

    void Start()
    {
        setGauge();
    }


    void Update()
    {
        setGauge();
        
        if (_pType == 1)
        {

            if (_gauge <= 50f)
            {
                objs[0].SetActive(true);

            }
            else if (_gauge <= 100f)
            {
                objs[0].SetActive(false);

                objs[1].SetActive(true);
            }
            else if (_gauge <= 150f)
            {
                objs[1].SetActive(false);
                objs[2].SetActive(true);
            }
            else
            {
                objs[2].SetActive(false);
                objs[3].SetActive(true);
            }
        }
        else if(_pType == 2) 
        {
            if (_gauge <= 50f)
            {
                objs[4].SetActive(true);

            }
            else if (_gauge <= 100f)
            {
                objs[4].SetActive(false);

                objs[5].SetActive(true);
            }
            else if (_gauge <= 150f)
            {
                objs[5].SetActive(false);
                objs[6].SetActive(true);
            }
            else
            {
                objs[6].SetActive(false);
                objs[7].SetActive(true);
            }
        }

      
    }
    

    public void setGauge()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<TextMeshPro>().SetText(_gauge.ToString() + '%');
            objs[i].SetActive(false);
        }
    }
}
