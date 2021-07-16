using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        offset = transform.position = target.position;

    }
}
