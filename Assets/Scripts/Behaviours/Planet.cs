using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9;
using MGJW9.JobySystem;
using System.Linq;
using DG.Tweening;

public class Planet : MonoBehaviour
{

    public List<Ship> inhabitants;

    public float orbitRadius = 2f;
    public float orbitSpeed = 1f;
    const float pi2 = 2 * Mathf.PI;
    public MeshRenderer playerOwnerMeshRenderer;
    public PlanetAnimation planetAnimation;
    int ownerPlayerId;
    Player previousOwner;
    Player playerOwner;

    const int minShipsToConquer = 0;

    private float spawnTimer;
    public float timeBetweenSpawns = 3f;
    private float consumptionUpdateRate = 1;
    private float consumptionUpdateTime = 0;

    private void Awake()
    {
        inhabitants = new List<Ship>();
        planetAnimation = GetComponent<PlanetAnimation>();
    }

    private void Start()
    {
        GameManager.instance.planets.Add(this);
        playerOwnerMeshRenderer.gameObject.SetActive(false);
        spawnTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        float alpha = 0;
        float beta = 0;

        for (int i = 0; i < 100; i++)
        {
            alpha = (float)i / 100f;
            beta = (float)(i+1) / 100f;
            Debug.DrawLine(GetOrbitPosition(alpha, 0), GetOrbitPosition(beta, 0), Color.yellow);
        }

        Debug.DrawLine(transform.position, GetOrbitPosition(0, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.25f, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.5f, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.75f, 0), Color.yellow);
    }

    public Vector3 GetOrbitPosition(float randomValue, float randomValue2)
    {
        var phase = Time.time * orbitSpeed + randomValue * pi2;
        var delta = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), Mathf.Sin(phase)).normalized;
        return transform.position + orbitRadius * delta * randomValue2;
    }

    private void OnTriggerEnter(Collider collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null && ship.currentTarget == this)
        {
            ship.arrivedAtTarget = true;
            inhabitants.Add(ship);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            inhabitants.Remove(ship);
        }
    }

    public void ComputePlanetOwner() 
    {
        if (inhabitants == null)
        {
            return;
        }

        for (int i = inhabitants.Count - 1; i >= 0; --i)
        {
            if (inhabitants[i] == null)
            {
                inhabitants.Remove(inhabitants[i]);
            }
        }


        var playersInhabitants = new Dictionary<int, int>();
        var currentWinnerPlayerId = -1;
        var currentWinnerNumShips = 0;
        for (int i = 0; i < inhabitants.Count; ++i)
        {
            var ship = inhabitants[i];
            var playerId = ship.owner.playerId;
            if (!playersInhabitants.ContainsKey(playerId))
            {
                playersInhabitants.Add(playerId, 0);
            }

            var numShipsPlayer = playersInhabitants[playerId];
            ++numShipsPlayer;

            if (numShipsPlayer > currentWinnerNumShips)
            {
                currentWinnerPlayerId = playerId;
                currentWinnerNumShips = numShipsPlayer;
            }

            playersInhabitants[playerId] = numShipsPlayer;
        }
        
        /*ownerPlayerId = inhabitants.GroupBy(ship => ship.owner != null ? ship.owner.playerId : 0)
            .OrderBy(group => group.Count())
            .Select(group => group.Key).FirstOrDefault();*/

        var result = GameManager.instance.players.SingleOrDefault(p => p.playerId == currentWinnerPlayerId);
        if (result != null)
        {
            previousOwner = playerOwner;
            playerOwner = result;
        }
        if (playerOwner != null)
        {
            planetAnimation.StartDrainingAnimation();

            playerOwnerMeshRenderer.gameObject.SetActive(true);
            playerOwnerMeshRenderer.material = playerOwner.playerPlanetMaterial;
        }
    }

    //private float 
    //private float cleanupTime = 5f;
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        consumptionUpdateTime += Time.deltaTime;
        if (spawnTimer >= timeBetweenSpawns && playerOwner != null && planetAnimation.currentPlanetEnergy > 0)
        {
            spawnTimer = 0f;
            //var newShip = ObjectPool.Spawn(GameManager.instance.shipPrefab, playerOwner.transform, transform.position);
            var newShip = Instantiate(GameManager.instance.shipPrefab, transform.position, Quaternion.identity, playerOwner.transform);
            var newShipComponent = newShip.GetComponent<Ship>();
            newShipComponent.SetOwner(playerOwner);
        }

        if (consumptionUpdateTime > consumptionUpdateRate) 
        {
            consumptionUpdateTime = consumptionUpdateRate - consumptionUpdateTime;
            // Update owner
            ComputePlanetOwner();
        }

      
    }

	private void ConquerBy(Player playerOwner)
	{
		foreach (var playerPlanetList in GameManager.instance.playerPlanets)
		{
			if (playerPlanetList.Value.Contains(this))
			{
				playerPlanetList.Value.Remove(this);
			}
		}

		GameManager.instance.playerPlanets[playerOwner.playerId].Add(this);
		planetAnimation.StartDrainingAnimation();
		playerOwnerMeshRenderer.gameObject.SetActive(true);
		playerOwnerMeshRenderer.material = playerOwner.playerPlanetMaterial;
		PlayersInfo.instance.playerInfo.ForEach(x => x.PrintPlanetsCount());
	}
}
