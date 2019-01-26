using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ShipCS : ComponentSystem
{

    struct Components
    {
        public Ship ship;
        public Mobile mobile;
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        /*float deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Components>())
        {
            var currentShip = entity.ship;
            var currentMobile = entity.mobile;

            if (currentShip.currentTarget == null)
            {
                currentShip.GetTarget();
            }

            if (currentShip.currentTarget != null)
            {
                Vector3 delta = currentShip.orbitPosition - entity.transform.position;
                currentMobile.accel = delta * currentMobile.thrust;
            }
        }*/
    }

}