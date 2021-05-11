using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 target = new Vector3(8, 1.5f, 0); //목표 위치
    // Update is called once per frame
    void Update()
    {
        
        //1.MoveTowards : 단순 등속 이동 (현재위치,목표위치,속도)
        transform.position = 
            Vector3.MoveTowards
            (transform.position,
            target, 1f);

        //2.SmoothDamp : 미끄러지듯이 감속 이동 (현재위치,목표위치,참조속도,속도)
        Vector3 velo = Vector3.zero;

        transform.position =
            Vector3.SmoothDamp
            (transform.position, 
            target, 
            ref velo,
            0.1f);

        //3.Lerp : 선형 보간(감속기간이 길다); 1f가 MAX
        transform.position =
            Vector3.Lerp
            (transform.position,
            target,
            0.01f);

        //4.SLerp : 구면 선형 보간; 포물선 이동
        transform.position =
            Vector3.Slerp
            (transform.position,
            target,
            0.01f);
    }
}
