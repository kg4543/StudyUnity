using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ob1_Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Vector3 target1 = new Vector3(-5, 2, -15); //현재 위치
    Vector3 target2 = new Vector3(5, 2, -15); //목표 위치
    // Update is called once per frame
    void Update()
    {
        //1.MoveTowards : 단순 등속 이동 (현재위치,목표위치,속도)

            transform.position = Vector3.MoveTowards
                                   (transform.position,
                                   target2, 0.1f);

            /*transform.position = Vector3.MoveTowards
                (transform.position,
                target1, 0.1f);*/
        
    }
}
