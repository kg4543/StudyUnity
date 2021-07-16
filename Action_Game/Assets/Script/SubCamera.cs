using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    float rotationX = 0.0f;
    float rotationY = 0.0f;

    public GameObject see;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(-rotationY,rotationX, 0.0f);
        transform.LookAt(see.transform.position);
    }
}
