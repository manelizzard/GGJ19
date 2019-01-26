using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9.JobySystem;

public class Ship : MonoBehaviour
{
    public Player owner;
    public Planet currentTarget;

    public Bullet bulletPrefab;
    public GameObject explosionPrefab;

    [Range(-1f, 1f)]
    public float shootAlignment = 0.5f;

    float randomValue;

    public float shootOffset = 0.5f;

    public Vector3 orbitPosition { get { return currentTarget.GetOrbitPosition(randomValue); } }

    private MeshRenderer meshRenderer;

    Bullet currentBullet;

    [HideInInspector]public Vector3 direction;

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
    
    public void GetTargetPlanet()
    {
        if (GameManager.instance.planets != null)
        {
            var planets = GameManager.instance.planets;
            currentTarget = planets[Random.Range(0, planets.Count)];
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var targetShip = collision.GetComponent<Ship>();

        if (targetShip != null)
        {
            var delta = ((Vector2)targetShip.transform.position - (Vector2)transform.position).normalized;
            var alignment = Vector2.Dot(delta.normalized, direction);

            if (alignment > shootAlignment)
            {
                if (currentBullet == null)
                {
                    currentBullet = Instantiate(bulletPrefab, transform.position, transform.rotation, transform) as Bullet;
                }
                currentBullet.shootOffset = shootOffset;
                currentBullet.target = collision.gameObject;
                Invoke("NullifyTarget", 0.2f);
            }
        }
    }

    void NullifyTarget()
    {
        currentBullet.CancelTarget();
        currentTarget = null;
    }

}
