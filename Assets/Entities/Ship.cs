using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public Player owner;
    public Planet currentTarget;

    float randomValue;

    public Vector3 orbitPosition { get { return currentTarget.GetOrbitPosition(randomValue); } }

    private void Awake()
    {
        randomValue = Random.value;
        transform.position += Random.insideUnitSphere;
    }

    public void GetTarget()
    {
        var planets = GameManager.instance.planets;
        currentTarget = planets[Random.Range(0, planets.Count)];
    }

}
