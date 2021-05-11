using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime,Space.World);
        // 축 방향으로 회전
    }

    
}
