using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject planetsParent;
    [HideInInspector]
    public List<Planet> planets;

	[HideInInspector]
	public List<Player> players;

    public Dictionary<int, List<Planet>> playerPlanets;
    private void Awake()
    {
        for (int i = 0; i < planetsParent.transform.childCount; i++) {
            planets.Add(planetsParent.transform.GetChild(i).gameObject.GetComponent<Planet>());
        }

        playerPlanets = new Dictionary<int, List<Planet>>();
        instance = this;
    }

}
