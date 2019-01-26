using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9.JobySystem;

public class Ship : MonoBehaviour
{
    public Player owner;
    public Planet currentTarget;

    public Bullet bulletPrefab;

    float randomValue;

    public Vector3 orbitPosition { get { return currentTarget.GetOrbitPosition(randomValue); } }

    private MeshRenderer meshRenderer;

    Bullet currentBullet;

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
        if (GameManager.instance.planets != null)
        {
            var planets = GameManager.instance.planets;
            currentTarget = planets[Random.Range(0, planets.Count)];
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (currentBullet == null)
    //    {
    //        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
    //        currentBullet.target = collision.gameObject;
    //    }
    //}

}
