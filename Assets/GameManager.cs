using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<Planet> planets;

    public Dictionary<int, List<Planet>> playerPlanets;
    private void Awake()
    {
        instance = this;
    }

}
