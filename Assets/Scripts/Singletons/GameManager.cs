﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get { if (instance_ == null) { instance_ = FindObjectOfType<GameManager>(); }; return instance_; } }
    static GameManager instance_;

    public Material arrowMaterial;
    public float arrowOffsetSpeed = 1f;

    public PlanetCursor cursor;
    public List<Planet> planets;

	[HideInInspector]
	public List<Player> players;

    public Dictionary<int, List<Planet>> playerPlanets;
    private void Awake()
    {
        playerPlanets = new Dictionary<int, List<Planet>>();
    }

    private void Update()
    {
        arrowMaterial.SetTextureOffset("_MainTex", new Vector2(-Time.time * arrowOffsetSpeed, 0));
    }

}
