using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startingShips = 10;
    public GameObject shipPrefab;

    // Instantiation
    private HFTInput hftInput;
    private HFTGamepad hftGamepad;

    // Data
    public string displayName = "Player";
    public int playerId = -1;
    public Color color = Color.blue;


    void Awake()
    {
        hftInput = GetComponent<HFTInput>();
        hftGamepad = GetComponent<HFTGamepad>();

        playerId = hftGamepad.playerName.GetHashCode();
        color = hftGamepad.color;

        // Assign random planet to spawned player
        GameManager.instance.playerPlanets.Add(playerId, new List<Planet>());
        GameManager.instance.playerPlanets[playerId].Add(GameManager.instance.planets[Random.Range(0, GameManager.instance.planets.Count)]);
    
        // Spawn ships
        SpawnShips();
    }    
    
    private void SpawnShips() 
    {
        Planet initialPlanet = GameManager.instance.playerPlanets[playerId][0];
        for(int i = 0; i < startingShips; i++) 
        {
            // Instantiate in first planet
            GameObject ship = Instantiate(shipPrefab, this.transform);
            ship.transform.position = initialPlanet.transform.position;
            ship.GetComponent<Ship>().currentTarget = initialPlanet;
        }
    }

    void Update()
    {
        bool buttonPressed = hftInput.GetButtonDown("fire1");
        if (buttonPressed)
        {
            // TODO: Make something when user presses the button
            Debug.Log(string.Format("User {0} pressed the button", hftGamepad.playerName));        
        }
    }
}
