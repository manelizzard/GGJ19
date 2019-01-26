using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9.JobySystem;

public class Ship : MonoBehaviour
{
    public Player owner;
    public Planet currentTarget;

    float randomValue;

    public Vector3 orbitPosition { get { return currentTarget.GetOrbitPosition(randomValue); } }

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        randomValue = Random.value;
        transform.position += Random.insideUnitSphere;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material) 
    {
        meshRenderer.material = material;
    }
    
    public void GetTarget()
    {
        var planets = GameController.instance.planets;
        currentTarget = planets[Random.Range(0, planets.Count)];
    }

}
