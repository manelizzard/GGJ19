using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startingShips = 10;
    public GameObject shipPrefab;

    public Material playerShipMaterial;
    public Material playerPlanetMaterial;

    // Instantiation
    private HFTInput hftInput;
    private HFTGamepad hftGamepad;

    // Data
    public string displayName = "Player";
    public int playerId = -1;
    public Color color = Color.blue;
    public List<Ship> ships;
    public Planet initialPlanet;
    public LineRenderer attackLineRenderer;

    Planet previousTarget;
    public Planet currentTarget;

    float lastCommandTime = -10;

    [HideInInspector]public Vector3 averagePosition;

    void Awake()
    {
        ships = new List<Ship>();
        hftInput = GetComponent<HFTInput>();
        hftGamepad = GetComponent<HFTGamepad>();

        playerId = GetInstanceID();
        color = hftGamepad.color;

        // Create a material copy
        playerShipMaterial = Material.Instantiate(playerShipMaterial);
        playerShipMaterial.SetColor("_Color", color);
        playerPlanetMaterial = Material.Instantiate(playerPlanetMaterial);
        playerPlanetMaterial.SetColor("_Color", color);
        attackLineRenderer.startColor = color;
        attackLineRenderer.endColor = color;

        // Assign random planet to spawned player
        GameManager.instance.playerPlanets.Add(playerId, new List<Planet>());
        GameManager.instance.playerPlanets[playerId].Add(GameManager.instance.planets[Random.Range(0, GameManager.instance.planets.Count)]);
    
        // Spawn ships
        SpawnShips();
    }

	private void Start()
	{
		GameManager.instance.players.Add(this);
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
            go.transform.position = initialPlanet.transform.position;
            Ship shipModel = go.GetComponent<Ship>();
            ships.Add(shipModel);
            shipModel.SetOwner(this);
            shipModel.SetMaterial(playerShipMaterial);
        }
    }

    void Update()
    {
        bool buttonPressed = hftInput.GetButtonDown("fire1") || Input.GetMouseButtonDown(0);
        if (buttonPressed && GameManager.instance.cursor.currentFocusedPlanet != null)
        {
            previousTarget = currentTarget;
            currentTarget = GameManager.instance.cursor.currentFocusedPlanet;
            foreach (Ship s in ships)
            {
                s.currentTarget = currentTarget;
            }
            attackLineRenderer.SetPosition(0, averagePosition);
            attackLineRenderer.SetPosition(1, currentTarget.transform.position);
            lastCommandTime = Time.time;
        }

        bool attackedRightNow = Time.time - lastCommandTime < 1f;
        attackLineRenderer.enabled = attackedRightNow;
    }

    private void LateUpdate()
    {
        averagePosition = Vector3.zero;
        foreach (var ship in ships)
        {
            if (ship != null)
            {
                averagePosition += ship.transform.position;
            }
        }
        averagePosition = averagePosition / ships.Count;
    }

}
