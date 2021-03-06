﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressSpawner : MonoBehaviour
{
    public GameObject shipPrefab;

    public int targetAmount = 10;

    void Update()
    {
        if (targetAmount != 0)
        {
            for (int i = 0; i < targetAmount; i++)
            {
                Instantiate(shipPrefab, transform.position + Random.insideUnitSphere*5f, Quaternion.identity);
            }
            targetAmount = 0;
        }
    }
}
