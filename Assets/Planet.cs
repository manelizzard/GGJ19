using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public float radius = 1f;
    public float orbitRadius = 2f;
    public float orbitSpeed = 1f;

    const float pi2 = 2 * Mathf.PI;

    private void Start()
    {
        GameManager.instance.planets.Add(this);
    }

    public Vector3 GetOrbitPosition(float randomValue)
    {
        var phase = Time.time * orbitSpeed + randomValue * pi2;
        var delta = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase));
        return transform.position + orbitRadius * delta;
    }

    public void ColonizedByPlayer(Player player) {
        // TODO: Make shader to identify planet as belonging to the player
    }

}
