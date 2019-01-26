﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int startingShips = 10;
	public GameObject shipPrefab;

	public Material playerMaterial;

	// Instantiation
	private HFTInput hftInput;
	private HFTGamepad hftGamepad;

	// Data
	public string displayName = "Player";
	public int playerId = -1;
	public Color color = Color.blue;
	public List<Ship> ships;
	public Planet initialPlanet;

	void Awake()
	{
		ships = new List<Ship>();
		hftInput = GetComponent<HFTInput>();
		hftGamepad = GetComponent<HFTGamepad>();

		playerId = GetInstanceID();
		color = hftGamepad.color;
		playerMaterial = new Material(Shader.Find("Specular"));
		playerMaterial.color = color;

		// Assign random planet to spawned player
		GameManager.instance.playerPlanets.Add(playerId, new List<Planet>());
		GameManager.instance.playerPlanets[playerId].Add(GameManager.instance.planets[Random.Range(0, GameManager.instance.planets.Count)]);

		// Spawn ships
		SpawnShips();
	}

	private void Start()
	{
		GameManager.instance.players.Add(this);
		GameManager.instance.PlayersInfo.AddPlayerInfo(this);
	}

	private void SpawnShips()
	{
		initialPlanet = GameManager.instance.playerPlanets[playerId][0];
		for (int i = 0; i < startingShips; i++)
		{
			// Instantiate in first planet
			GameObject go = Instantiate(shipPrefab, this.transform);
			go.transform.position = initialPlanet.transform.position;
			Ship shipModel = go.GetComponent<Ship>();
			ships.Add(shipModel);
			shipModel.SetOwner(this);
			shipModel.SetMaterial(playerMaterial);
		}
		GameManager.instance.PlayersInfo.GetPlayerInfo(playerId).PrintShipCount();
		GameManager.instance.PlayersInfo.GetPlayerInfo(playerId).PrintPlanetsCount();
	}

	void Update()
	{
		bool buttonPressed = hftInput.GetButtonDown("fire1");
		if (buttonPressed)
		{
			// TODO: Make something when user presses the button
			Debug.Log(string.Format("User {0} pressed the button", hftGamepad.playerName));
			foreach (Ship s in ships)
			{
				s.currentTarget = GameManager.instance.cursor.currentFocusedPlanet;
			}
		}
	}
}
