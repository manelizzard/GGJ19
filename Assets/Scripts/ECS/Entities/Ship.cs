using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9.JobySystem;

public class Ship : MonoBehaviour
{
    public Planet currentTarget;

    public Bullet bulletPrefab;
    public GameObject explosionPrefab;
    public TrailRenderer trailRenderer;
    public SpriteRenderer haloRenderer;

    [Range(-1f, 1f)]
    public float shootAlignment = 0.5f;

    public Player owner { get; set; }
    float randomValue, randomValue2;

    public float shootOffset = 0.5f;
    public float avoidRadius = 0.5f;
    public float avoidThrust = 1f;

    public Vector3 orbitPosition { get { return currentTarget.GetOrbitPosition(randomValue, randomValue2); } }

    private MeshRenderer meshRenderer;

    Bullet currentBullet;

    [HideInInspector]public Vector3 direction;

    private void Awake()
    {
        randomValue = Random.value;
        randomValue2 = Random.value;
        transform.position += Random.insideUnitSphere;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material) 
    {
        meshRenderer.material = material;
    }
    
    public void GoToRandomPlanet()
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
            if (owner != null)
            {
                owner.ships.Remove(this);
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var targetShip = collision.GetComponent<Ship>();

        if (targetShip != null && targetShip.owner != this.owner)
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

    public void SetOwner(Player player)
    {
        owner = player;
        currentTarget = player.initialPlanet;
        trailRenderer.startColor = new Color(player.color.r, player.color.g, player.color.b, trailRenderer.startColor.a);
        trailRenderer.endColor = new Color(player.color.r, player.color.g, player.color.b, trailRenderer.startColor.a);
        haloRenderer.color = new Color(player.color.r, player.color.g, player.color.b, haloRenderer.color.a);
    }

    void NullifyTarget()
    {
        currentBullet.CancelTarget();
        currentTarget = null;
    }

}
