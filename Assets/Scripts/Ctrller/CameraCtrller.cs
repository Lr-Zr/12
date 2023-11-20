using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class CameraCtrller : MonoBehaviour
{
    [SerializeField]
    GameObject go1 = null;
    [SerializeField]
    GameObject go2 = null;


    Vector3 pos1;
    Vector3 pos2;
    Vector3 camerapos;
    void Start()
    {
        
    }

   
    void FixedUpdate()
    {
        if (go1 == null|| go2 == null)
        {
            this.pos2 = transform.position;
            return;
        }
        pos1 = go1.transform.position;
        pos2 = go2.transform.position;


        camerapos.x = (pos1.x + pos2.x)*0.5f;
        //카메라 최대 x값
        if (camerapos.x > 12.0f)
            camerapos.x = 12f;
        else if (camerapos.x < -12.0f)
            camerapos.x = -12f;



        camerapos.z = -10*Mathf.Abs(pos1.x-pos2.x)*Time.deltaTime -7  ;
        if (camerapos.z < -18.0f)
            camerapos.z = -18f;
        else if (camerapos.z > -3.0f)
            camerapos.z = -3f;
       

        camerapos.y = (pos1.y+pos2.y) * 0.3f - camerapos.z/2.0f -1.0f;
        if (camerapos.y > 9.0f)
            camerapos.y = 9f;
        else if (camerapos.y < 3.0f)
            camerapos.y = 3f;
        this.transform.position = camerapos;
    }
}
