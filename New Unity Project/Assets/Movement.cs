using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Time.deltaTime : 1 프레임의 시간 (사용자들간의 시간을 맞춰줌)
        // Translate : 백터에 곱하기
        // Vector 함수 : 시간 매개변수에 곱하기

        Vector3 vec = new Vector3(
            (Input.GetAxis("Horizontal") * Time.deltaTime * 3),
            0, (Input.GetAxis("Vertical") * Time.deltaTime * 3)); //벡터 값
        transform.Translate(vec);

        /*Vector3 vec = new Vector3(
            (Input.GetAxis("Horizontal")) / 10,
            0, (Input.GetAxis("Vertical")) / 10); //벡터 값
        transform.Translate(vec);*/

        if (Input.GetButtonUp("Jump"))
        {
            Vector3 dow = new Vector3(
            0, -1, 0);
            transform.Translate(dow);
        }
        if (Input.GetButtonDown("Jump"))
        {
            Vector3 jum = new Vector3(
                        0, 1, 0);
            transform.Translate(jum);
        }

    }
}
