using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MobileCS : ComponentSystem
{

    struct Components
    {
        public Mobile mobile;
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Components>())
        {
            var currentMobile = entity.mobile;

            currentMobile.speed += currentMobile.accel * deltaTime;
            currentMobile.speed = currentMobile.speed.normalized * Mathf.Min(currentMobile.speed.magnitude, currentMobile.maxSpeed);

            currentMobile.position += currentMobile.speed * deltaTime;
            entity.transform.position = currentMobile.position;
            entity.transform.LookAt(entity.transform.position + currentMobile.speed);

            currentMobile.accel = Vector3.zero;
        }
    }

}