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
            Input.GetAxis("Vertical"), 0); //���� ��
        transform.Translate(vec);

        /*if (Input.anyKeyDown)
        {
            Debug.Log("Akey");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("�������� ����");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("�������� �̵�");
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("���������� �̵� ����");
        }

        if (Input.GetMouseButtonDown(0)) // 0:���� 1:������
        {
            Debug.Log("����");
        }
        if (Input.GetMouseButton(0))
        {
            Debug.Log("�� ������ ��...");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("����");
        }*/

        /*if (Input.GetButtonDown("Jump")) 
        {
            Debug.Log("����");
        }
        if (Input.GetButton("Jump"))
        {
            Debug.Log("�� ������ ��...");
        }
        if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("��������!");
        }

        if (Input.GetButton("Horizontal")) 
        {
            Debug.Log("Ⱦ �̵� ��..." 
                + Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetButton("Vertical"))
        {
            Debug.Log("�� �̵� ��..."
                + Input.GetAxisRaw("Vertical"));
        }*/
    }
}
