using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlanetCS : ComponentSystem
{

    struct Components
    {
        public Planet planet;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Components>())
        {
            
        }
    }

}