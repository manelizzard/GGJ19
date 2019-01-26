using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CollisionCS : ComponentSystem
{

    struct Components
    {
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Components>())
        {
            
        }
    }

}