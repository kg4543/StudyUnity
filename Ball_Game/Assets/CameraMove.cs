using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform playerTransform;
    Vector3 offset;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //CPU 부하로 권장하지는 않음
        offset = transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
