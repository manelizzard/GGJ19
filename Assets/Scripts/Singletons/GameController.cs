using System.Collections;
using System.Collections.Generic;
using MGJW9.JobySystem;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace MGJW9.JobSystem
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance { get { if (instance_ == null) { instance_ = FindObjectOfType<GameController>(); }; return instance_; } }
        static GameController instance_;

        [HideInInspector]
        public List<Planet> planets;

        public GameObject shipPrefab;

        TransformAccessArray transforms;
        MovementJob moveJob;
        JobHandle moveHandle;

        public List<Ship> ships;

        private void Start()
        {
            transforms = new TransformAccessArray(0, -1);
            AddShips(10000);
        }

        private void OnDisable()
        {
            moveHandle.Complete();
            transforms.Dispose();
        }

        private void Update()
        {
            var velocity = new NativeArray<Vector3>(500, Allocator.TempJob);
            for (var i = 0; i < velocity.Length; ++i)
            {
                velocity[i] = Vector3.zero;
            }

            var acceleration = new NativeArray<Vector3>(500, Allocator.TempJob);
            for (var i = 0; i < acceleration.Length; ++i)
            {
                acceleration[i] = Vector3.zero;
            }

            var maximumSpeed = new NativeArray<float>(500, Allocator.TempJob);
            for (var i = 0; i < maximumSpeed.Length; ++i)
            {
                maximumSpeed[i] = 5f;
            }

            var orbitPosition = new NativeArray<Vector3>(500, Allocator.TempJob);
            for (int i = 0; i < orbitPosition.Length; ++i)
            {
                var ship = ships[i];
                if (ship.currentTarget == null)
                {
                    ship.GoToRandomPlanet();
                }
                orbitPosition[i] = ship.orbitPosition;
            }

            //Here we create the job
            moveJob = new MovementJob()
            {
                maxSpeed = maximumSpeed,
                moveSpeed = velocity,
                moveAccel = acceleration,
                orbitPosition = orbitPosition,
                deltaTime = Time.deltaTime
            };

            moveHandle = moveJob.Schedule(transforms);
            JobHandle.ScheduleBatchedJobs();

            //To keep in sync the update of the job with the general update,
            //we ask the job to complete before doing anything else
            moveHandle.Complete();

            velocity.Dispose();
            acceleration.Dispose();
            maximumSpeed.Dispose();
            orbitPosition.Dispose();
        }

        private void AddShips(int amount)
        {
            moveHandle.Complete();
            transforms.capacity = transforms.length + amount;

            ships = new List<Ship>();
            for (int i = 0; i < amount; ++i)
            {
                float xVal = Random.Range(-3f, 3f);
                float zVal = Random.Range(-3f, 3f);

                var pos = new Vector3(xVal, 0f, zVal);

                var shipObj = Instantiate<GameObject>(shipPrefab, pos, Quaternion.identity);
                transforms.Add(shipObj.transform);
                ships.Add(shipObj.GetComponent<Ship>());
            }
        }
    }
}
