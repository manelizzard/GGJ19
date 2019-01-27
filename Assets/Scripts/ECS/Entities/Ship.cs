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
    public float thrust = 1f;

    public float baseMaxSpeed = 1f;
    public float deltaMaxSpeed = 0.1f;
    public float maxSpeed { get { return baseMaxSpeed + (Random.value - 0.5f) * deltaMaxSpeed * 2f; } }

    public Vector3 orbitPosition { get { if (currentTarget != null) { return currentTarget.GetOrbitPosition(randomValue, randomValue2); } else { return transform.position; } } }

    private MeshRenderer meshRenderer;

    Bullet currentBullet;
    Rigidbody rb;

    [System.NonSerialized]public bool arrivedAtTarget = true;

    public Vector3 direction { get { return rb.velocity.normalized; } }

    Vector3 randomAxis;

    private void Awake()
    {
        randomValue = Random.value;
        randomValue2 = Random.value;
        randomAxis = Random.insideUnitSphere + new Vector3(-1,1,-1);
        transform.position += Random.insideUnitSphere;
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere.normalized*maxSpeed;
    }

    private void Update()
    {
        if (owner == null && currentTarget == null)
        {
            GoToRandomPlanet();
        }

        if (owner != null && currentTarget == null)
        {
            currentTarget = owner.currentTarget;
        }
    }

    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            Vector3 delta = (currentTarget.transform.position - transform.position).normalized;
            if (arrivedAtTarget)
            {
                rb.AddForce(delta * thrust * (0.7f + randomValue2));
                rb.AddForce(Vector3.Cross(randomAxis, delta) * thrust * 0.7f);
            }
            else
            {
                rb.AddForce(delta * thrust);
                transform.LookAt(transform.position + rb.velocity);
            }
        }

        rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude, maxSpeed);
        transform.LookAt(transform.position + rb.velocity);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, orbitPosition, Color.cyan);
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
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (currentTarget != null && arrivedAtTarget)
            {
                if (currentTarget.inhabitants.Contains(this))
                {
                    //currentTarget.inhabitants.Remove(this);
                    //currentTarget.ComputePlanetOwner();
					//PlayersInfo.instance.playerInfo.Find(x => x.playerId == owner.playerId).PrintShipCount();
				}
			}

            if (owner != null)
            {
                owner.ships.Remove(this);
				PlayersInfo.instance.playerInfo.Find(x => x.playerId == owner.playerId).PrintShipCount();
				if(owner.shipCount <= 0)
				{
					RemovedPlayer();
				}
			}
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        var targetShip = collision.GetComponent<Ship>();

        if (targetShip != null && targetShip.owner != this.owner)
        {
            var delta = (targetShip.transform.position - transform.position).normalized;
            var alignment = Vector3.Dot(delta.normalized, direction);

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
        owner.ships.Add(this);
        currentTarget = player.currentTarget;
        trailRenderer.startColor = new Color(player.color.r, player.color.g, player.color.b, trailRenderer.startColor.a);
        trailRenderer.endColor = new Color(player.color.r, player.color.g, player.color.b, trailRenderer.startColor.a);
        haloRenderer.color = new Color(player.color.r, player.color.g, player.color.b, haloRenderer.color.a);
        SetMaterial(player.playerShipMaterial);
    }

    void NullifyTarget()
    {
        currentTarget.inhabitants.Remove(this);
        currentTarget.ComputePlanetOwner();
        currentBullet.CancelTarget();
        //currentTarget = null;
    }

	void RemovedPlayer()
	{
		if(GameManager.instance.playerPlanets[owner.playerId].Count <= 0)
		{
			//This player can't play anymore
			owner.RemoveThisPlayer();
		}
		else
		{
			bool remove = true;
			foreach(Planet planet in GameManager.instance.playerPlanets[owner.playerId])
			{
				remove &= planet.planetAnimation.currentPlanetEnergy == 0;
			}

			if (remove)
			{
				//This player can't play anymore
				owner.RemoveThisPlayer();
			}
		}
	}

}
