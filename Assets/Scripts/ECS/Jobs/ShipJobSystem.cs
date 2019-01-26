using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;
using Unity.Collections;

namespace MGJW9.JobySystem
{
    [Unity.Burst.BurstCompile]
    public struct MovementJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float> maxSpeed;

        [ReadOnly]
        public NativeArray<Vector3> orbitPosition;

        [ReadOnly]
        public float deltaTime;

        public NativeArray<Vector3> moveSpeed;
        public NativeArray<Vector3> moveAccel;

        public void Execute(int index, TransformAccess transform)
        {
            var delta = orbitPosition[index] - transform.position;
            var mAccel = moveAccel[index];
            mAccel += delta * 10f;
            moveAccel[index] = mAccel;

            var mSpeed = moveSpeed[index];
            mSpeed += moveAccel[index] * deltaTime;
            mSpeed = mSpeed.normalized *
                                Mathf.Min(mSpeed.magnitude, maxSpeed[index]);
            moveSpeed[index] = mSpeed;

            var pos = transform.position;
            pos += moveSpeed[index] * deltaTime;
            transform.position = pos;

            moveAccel[index] = Vector3.zero;
        }
    }

}
