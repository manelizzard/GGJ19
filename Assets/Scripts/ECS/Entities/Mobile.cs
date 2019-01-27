using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
    public Vector3 position;
    public Vector3 speed;
    public Vector3 accel;
    public float thrust = 1f;


    private void Start()
    {
        position = transform.position;
        speed = Vector3.zero;
        accel = Vector3.zero;
    }
}
