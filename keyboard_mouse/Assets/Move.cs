using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Update is called once per frame

    private void Start()
    {
        
    }

    void Update()
    {
        Vector3 vec = new Vector3(
            Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical"), 0); //벡터 값
        transform.Translate(vec);

        /*if (Input.anyKeyDown)
        {
            Debug.Log("Akey");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("아이템을 구입");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("왼쪽으로 이동");
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("오른쪽으로 이동 중지");
        }

        if (Input.GetMouseButtonDown(0)) // 0:왼쪽 1:오른쪽
        {
            Debug.Log("공격");
        }
        if (Input.GetMouseButton(0))
        {
            Debug.Log("기 모으는 중...");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("공격");
        }*/

        /*if (Input.GetButtonDown("Jump")) 
        {
            Debug.Log("점프");
        }
        if (Input.GetButton("Jump"))
        {
            Debug.Log("기 모으는 중...");
        }
        if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("슈퍼점프!");
        }

        if (Input.GetButton("Horizontal")) 
        {
            Debug.Log("횡 이동 중..." 
                + Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetButton("Vertical"))
        {
            Debug.Log("종 이동 중..."
                + Input.GetAxisRaw("Vertical"));
        }*/
    }
}
