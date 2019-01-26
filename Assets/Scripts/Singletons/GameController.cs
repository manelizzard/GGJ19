using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace MGJW9.JobySystem
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
            //To keep in sync the update of the job with the general update,
            //we ask the job to complete before doing anything else
            moveHandle.Complete();

            //Here we create the job
            moveJob = new MovementJob()
            {
                moveSpeed = 2f,
                deltaTime = Time.deltaTime
            };

            moveJob.Schedule(transforms);
            JobHandle.ScheduleBatchedJobs();
        }

        private void AddShips(int amount)
        {
            moveHandle.Complete();
            transforms.capacity = transforms.length + amount;

            for (int i = 0; i < amount; ++i)
            {
                float xVal = Random.Range(-3f, 3f);
                float zVal = Random.Range(-3f, 3f);

                var pos = new Vector3(xVal, 0f, zVal);

                var shipObj = Instantiate<GameObject>(shipPrefab, pos, Quaternion.identity);
                transforms.Add(shipObj.transform);
            }
        }
    }
}
