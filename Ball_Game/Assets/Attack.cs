using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Vector3 now = new Vector3(-6, 0, 5); // 현재위치
    Vector3 target = new Vector3(3, 0, 5); // 목표위치
    bool move = false;
    // Update is called once per frame
    void Update()
    {
        //1.MoveTowards : 단순 등속 이동 (현재위치,목표위치,속도)
        if (move == false)
        {
            transform.position = Vector3.MoveTowards
                                           (transform.position,
                                           target, 0.1f);
        }
        else
        {
            transform.position = Vector3.MoveTowards
                                           (transform.position,
                                           now, 0.1f);
            
        }

        if (transform.position == target)
        {
            move = true;
        }
        else if (transform.position == now)
        {
            move = false;
        }
    }
}
