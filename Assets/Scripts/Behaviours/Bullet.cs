using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public LineRenderer lineRenderer;

    void Start()
    {
        
    }

    //void Update()
    //{
    //    if (target != null)
    //    {
    //        lineRenderer.SetPosition(0, transform.position);
    //        lineRenderer.SetPosition(1, target.transform.position);
    //    }
    //}
}
