using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 0.125f;

    void Start()
    {
        var position = target.position;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    void LateUpdate()
    {
        var position = target.position;
        var newPosition = new Vector3(position.x, position.y, transform.position.z);
        // transform.position = new Vector3(position.x, position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
    }
}