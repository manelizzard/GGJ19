using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int startingShips = 10;
	public GameObject shipPrefab;

	public Material playerMaterial;

	public Material playerShipMaterial;
	public Material playerPlanetMaterial;

	// Data
	public string displayName = "Player";
	public int playerId = -1;
	public Color color = Color.blue;
	public List<Ship> ships;
	public Planet initialPlanet;

	public LineRenderer attackLineRenderer;

	Planet previousTarget;
	public Planet currentTarget;

	HFTInput hftInput;
	HFTGamepad hftGamepad;

	float lastCommandTime = -10;

	[HideInInspector] public int shipCount = 0;
	[HideInInspector] public Vector3 averagePosition;

	internal bool removed = false;

    public static int playersTargetedToCurrentPlanet;

	private void Awake()
	{
		hftInput = GetComponent<HFTInput>();
		hftGamepad = GetComponent<HFTGamepad>();
		playerId = GetInstanceID();
		color = hftGamepad.color;
		playerMaterial = new Material(Shader.Find("Specular"));
		playerMaterial.color = color;

		// Create a material copy
		playerShipMaterial = Material.Instantiate(playerShipMaterial);
		playerPlanetMaterial = Material.Instantiate(playerPlanetMaterial);
		playerShipMaterial.SetColor("_Color", color);
		playerPlanetMaterial.SetColor("_Color", color);
		attackLineRenderer.startColor = color;
		attackLineRenderer.endColor = color;
		GameManager.instance.playerPlanets.Add(playerId, new List<Planet>());

		var planetIndex = -1;
		for (var i = 0; i < GameManager.instance.planets.Count; ++i)
		{
			var planet = GameManager.instance.planets[i];
			if (planet.playerOwner == null)
			{
				planetIndex = i;
				break;
			}
		}

		if (planetIndex != -1)
		{
			GameManager.instance.playerPlanets[playerId].Add(GameManager.instance.planets[planetIndex]);

			// Spawn ships
			SpawnShips();
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	private void Start()
	{
		GameManager.instance.players.Add(this);
		GameManager.instance.PlayersInfo.AddPlayerInfo(this);
	}


	private void SpawnShips()
	{
		initialPlanet = GameManager.instance.playerPlanets[playerId][0];
		previousTarget = initialPlanet;
		currentTarget = initialPlanet;

		for (int i = 0; i < startingShips; i++)
		{
			// Instantiate in first planet
			GameObject go = Instantiate(shipPrefab, this.transform);
			go.transform.position = initialPlanet.transform.position + Random.insideUnitSphere * initialPlanet.orbitRadius * 0.25f;
			Ship shipModel = go.GetComponent<Ship>();
			//ships.Add(shipModel);
			shipModel.SetOwner(this);
		}
	}

	void Update()
	{
		bool buttonPressed = hftInput.GetButtonDown("fire1") || Input.GetMouseButtonDown(0);
		if (buttonPressed && GameManager.instance.cursor.currentFocusedPlanet != null)
		{
			previousTarget = currentTarget;
			currentTarget = GameManager.instance.cursor.currentFocusedPlanet;
			foreach (Ship ship in ships)
			{

				ship.currentTarget = currentTarget;
				ship.arrivedAtTarget = false;
			}
			attackLineRenderer.SetPosition(0, averagePosition);
			attackLineRenderer.SetPosition(1, currentTarget.transform.position);
			lastCommandTime = Time.time;
		}

		if (ships.Count <= 0)
		{
			if (GameManager.instance.playerPlanets[playerId].Count <= 0)
			{
				//This player can't play anymore
				RemoveThisPlayer();
			}
			else
			{
				bool remove = true;
				foreach (Planet planet in GameManager.instance.playerPlanets[playerId])
				{
					remove &= planet.planetAnimation.currentPlanetEnergy == 0;
				}

				if (remove)
				{
					//This player can't play anymore
					RemoveThisPlayer();
				}
			}
		}

		bool attackedRightNow = Time.time - lastCommandTime < 1f;
		attackLineRenderer.enabled = attackedRightNow && !removed;
	}

	private void LateUpdate()
	{
		averagePosition = Vector3.zero;
		shipCount = 0;
		foreach (var ship in ships)
		{
			if (ship != null)
			{
				shipCount++;
				averagePosition += ship.transform.position;
			}
		}
		if (shipCount != 0)
		{
			averagePosition = averagePosition / shipCount;
		}
		else
		{
			averagePosition = Vector3.zero;
		}
	}

	public void RemoveThisPlayer()
	{
		Debug.Log("Player removed " + playerId);
		GameManager.instance.removedPlayers.Add(this);
		removed = true;
		if (GameManager.instance.removedPlayers.Count == GameManager.instance.players.Count - 1)
		{
			//Game Over
			GameOverPanel.instance.GameOver();
		}
	}

}
