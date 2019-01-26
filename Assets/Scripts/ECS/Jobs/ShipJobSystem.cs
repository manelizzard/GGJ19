using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;

namespace MGJW9.JobySystem
{
    [Unity.Burst.BurstCompile]
    public struct MovementJob : IJobParallelForTransform
    {
        public float moveSpeed;
        public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            var pos = transform.position;
            pos += moveSpeed * deltaTime * (transform.rotation * Vector3.left);
            transform.position = pos;
        }
    }

}
