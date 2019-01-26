using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public LineRenderer lineRenderer;
    public float shootOffset;

    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            lineRenderer.SetPosition(0, transform.position + shootOffset * transform.forward);
            lineRenderer.SetPosition(1, target.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    internal void CancelTarget()
    {
        if (target!=null)
        {
            Destroy(target.gameObject);
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        target = null;
    }
}
