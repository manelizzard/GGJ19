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
        float deltaTime = Time.deltaTime;
        var shipsMask = LayerMask.GetMask("Ships");

        foreach (var entity in GetEntities<Components>())
        {
            var currentShip = entity.ship;
            var currentMobile = entity.mobile;

            currentShip.direction = ((Vector2)entity.mobile.speed).normalized;

            if (currentShip.owner == null && currentShip.currentTarget == null)
            {
                currentShip.GoToRandomPlanet();
            }

            if (currentShip.currentTarget != null)
            {
                Vector3 delta = currentShip.orbitPosition - entity.transform.position;
                currentMobile.accel += delta * currentMobile.thrust;
            }

            var neighbours = Physics2D.OverlapCircleAll(entity.transform.position, currentShip.avoidRadius, shipsMask );

            foreach (var neighbour in neighbours)
            {
                if (neighbour.gameObject == entity.transform.gameObject) continue;
                Vector3 separation = neighbour.transform.position - entity.transform.position;
                if (separation.magnitude<currentShip.avoidRadius)
                {
                    currentMobile.accel += separation.normalized * currentShip.avoidThrust;
                }
            }

        }
    }

}